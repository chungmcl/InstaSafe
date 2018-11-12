import json
import math
import requests
from bs4 import BeautifulSoup
import os
import subprocess
from hatesonar import Sonar
import datetime as dt
import emoji
import ctypes
ctypes.windll.user32.ShowWindow( ctypes.windll.kernel32.GetConsoleWindow(), 6 )


#gets page html
def get_html(url):
    response = requests.get(url)
    return response.text

def extract_json_data(html):
    soup = BeautifulSoup(html, "html.parser")
    #saves everything in <body> to body var
    body = soup.find("body")

    #saves everything in <script> within <body> to script_tag var
    script_tag = body.find("script")

    #removes whitespace from beginning and end of script text
    raw_string = script_tag.text.strip()

    #deletes any text that says "window._sharedData =" in the script text
    raw_string = raw_string.replace("window._sharedData =", "")

    #deletes any semicolons in script text
    raw_string = raw_string.replace(";", "")

    return json.loads(raw_string)

def get_profile_full_name(profile_username):
    #gets page html for profile
    profile_html = get_html("https://instagram.com/" + profile_username)

    #gets json data for profile
    json_data = extract_json_data(profile_html)

    #saves profile metrics to metrics var
    metrics = json_data["entry_data"]["ProfilePage"][0]["graphql"]["user"]

    #if key is full_name return the value (which is the profile owners full name)
    for key, value in metrics.items():
        if(key == "full_name"):
            return value

def get_profile_recent_posts(profile_username):
    #gets page html for profile
    profile_html = get_html("https://instagram.com/" + profile_username)

    #gets json data for profile
    json_data = extract_json_data(profile_html)

    #saves profile metrics to metrics var
    return json_data["entry_data"]["ProfilePage"][0]["graphql"]["user"]['edge_owner_to_timeline_media']["edges"]

def get_profile_captions(profile_username):
    metrics = get_profile_recent_posts(profile_username)

    captions = []

    #gets caption for each post
    for post in metrics:
        post = post.get("node")

        #gets container the caption is in and converts to string
        caption_container = post.get("edge_media_to_caption").get("edges")
        caption_container = str(caption_container[0])

        #substrings caption container to just be the caption
        caption = caption_container[caption_container.index(":") + 1 :]
        caption = caption[caption.index(":") + 1 :]
        caption = caption[caption.index("\'") + 1 : caption.index("}") - 1]
        caption = emoji.demojize(caption)
        #adds caption to array of captions
        captions.append(caption)

    return captions

def get_profile_picture_urls(profile_username):
    metrics = get_profile_recent_posts(profile_username)

    picture_urls = []

    #gets caption for each post
    for post in metrics:
        post = post.get("node")

        #gets container the image url is in and converts to string
        current_pic_url = post.get("display_url")
        picture_urls.append(current_pic_url)
    return picture_urls

def get_profile_picture_dates(profile_username):
    metrics = get_profile_recent_posts(profile_username)

    picture_dates = []

    #gets caption for each post
    for post in metrics:
        post = post.get("node")
        current_time_stamp = post.get("taken_at_timestamp")
        picture_dates.append(dt.datetime.fromtimestamp(current_time_stamp).strftime('%m/%d/%Y'))
    return picture_dates

#image classification functions

def formatTag(self):
    line = self[17:len(self)]
    split_text = line.split("\"")
    line_final = split_text[3].replace(" ", "_") + " " + '{0:.{1}f}'.format(float(split_text[0].replace(",", "")), 2)
    return line_final

def getRawImageData(self):
    url = "http://api.imagga.com/v1/tagging"
    querystring = {"url":self,"version":"2"}
    headers = {
        'accept': "application/json",
        'authorization': "Basic YWNjXzU0ZGExZjg4Y2ZjYjgzMDphMzUwNTFhZThhZTVjMjExMmI4ZThlNmY1ZDRmODRkMw=="
        }
    response = requests.request("GET", url, headers=headers, params=querystring)
    output_raw = response.text
    output_formatted = ""
    output_raw_split = output_raw.split("}")
    for i in range (1, len(output_raw_split) - 3):
        tag_formatted = formatTag(output_raw_split[i])
        #if float(tag_formatted.split(" ")[1]) >= 10.00:
        output_formatted += tag_formatted + ","
    output_list = output_formatted.split(",")
    output_list.pop()
    return output_list

def assessViolence(self):
    threat_level = 0
    flag_words = open("violentTags.txt", "r").read().split(", ")
    for i in range (0, 10):
        for n in range (0, len(flag_words)):
            if flag_words[n] in self[i].split(" ")[0]:
                threat_level += float(self[i].split(" ")[1])
    if threat_level > 0:
        return '{0:.{1}f}'.format(math.log10(threat_level) * 25, 2)
    else:
        return 0
    #return threat_level / 10

def assessImages(self):
    urls = open(self, "r").read().split(", ")
    for i in range (0, len(urls)):
        print(assessViolence(getRawImageData(urls[i])))

def getImageAccountData(self):
    file_input = open(self, "r").readlines()
    file_output = open("ImageData.txt", "w")
    i = 0;
    while i < len(file_input):
        i += 1
        file_output.write(file_input[i])
        i += 1
        while len(file_input[i]) > 1:
            file_output.write(file_input[i].split(" ")[0] + " " + str('{0:.{1}f}'.format(float(assessViolence(getRawImageData(file_input[i].split(" ")[1]))), 2)) + "\n")
            i += 1
            if i == len(file_input):
                file_output.close()
                return
    file_input.close()
    file_output.close()

#Natural Language Processing functions

def getOffensiveness(self):
    sonar = Sonar()
    data_raw = str(sonar.ping(self))
    data_raw_split = data_raw.split(": [{")[1].split("}]}")[0].replace("'class_name': '", "").replace("', 'confidence':", "").split("}, {")
    output = data_raw_split[0].split(" ")[0] + " "
    if "e" in data_raw_split[0].split(" ")[1]:
        output += "0.00"
    else:
        output += str('{0:.{1}f}'.format(float(data_raw_split[0].split(" ")[1]) * 100, 2)) + " "
    output += data_raw_split[1].split(" ")[0] + " "
    if "e" in data_raw_split[1].split(" ")[1]:
        output += "0.00"
    else:
        output += str('{0:.{1}f}'.format(float(data_raw_split[1].split(" ")[1]) * 100, 2)) + " "
    output += data_raw_split[2].split(" ")[0] + " "
    if "e" in data_raw_split[2].split(" ")[1]:
        output += "0.00"
    else:
        output += str('{0:.{1}f}'.format(float(data_raw_split[2].split(" ")[1]) * 100, 2))
    return (output)

def getCaptionAccountData(self):
    file_input = open(self, "r").readlines()
    file_output = open("CaptionData.txt", "w")
    i = 0;
    while i < len(file_input):
        i += 1
        file_output.write(file_input[i])
        i += 1
        while len(file_input[i]) > 1:
            file_output.write(getOffensiveness(file_input[i]) + "\n")
            i += 1
            if i == len(file_input):
                file_output.close()
                return
    file_input.close()
    file_output.close()

abspath = os.path.abspath(__file__)
dname = os.path.dirname(abspath)
os.chdir(dname)

#opens file of usernames
usernames_file = open("usernames.txt")
usernames_text = usernames_file.read()

#outputs each caption to captions.txt file and each url to urls.txt file
captions_file = open("captions.txt", "w")
image_urls_file = open("urls.txt", "w")
for i in range(usernames_text.count(";")):
    #parses current username from usernames.txt file
    current_username = usernames_text[: usernames_text.index(";")]
    usernames_text = usernames_text[usernames_text.index(";") + 2 :]

    #writes captions to captions.txt
    captions_file.write("\n" + current_username + "\n")
    for caption in get_profile_captions(current_username):
        try:
            captions_file.write(caption + "\n")
        except:
            for n in range (0, len(caption)):
                try:
                    captions_file.write(caption[n])
                except:
                    captions_file.write("")
            captions_file.write("\n")

    #writes urls to urls.txt
    image_urls_file.write("\n" + current_username + "\n")
    urls_array = get_profile_picture_urls(current_username)
    dates_array = get_profile_picture_dates(current_username)
    for i in range(len(urls_array)):
        image_urls_file.write(dates_array[i] + " " + urls_array[i] + "\n")

captions_file.close()
image_urls_file.close()


#Image Classification
abspath = os.path.abspath(__file__)
dname = os.path.dirname(abspath)
os.chdir(dname)
getImageAccountData("urls.txt")

#Natural Language Processing
abspath = os.path.abspath(__file__)
dname = os.path.dirname(abspath)
os.chdir(dname)
getCaptionAccountData("captions.txt")

complete = open("complete.txt", "w")
complete.write("complete")
complete.close()
