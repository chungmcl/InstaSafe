import json
import requests
from bs4 import BeautifulSoup

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

#opens file of usernames
usernames_file = open("usernames.txt")
usernames_text = usernames_file.read()

#outputs each caption to captions.txt file and each url to urls.txt file
captions_file = open("captions.txt", "a")
image_urls_file = open("urls.txt", "a")
for i in range(usernames_text.count(";")):
    #parses current username from usernames.txt file
    current_username = usernames_text[: usernames_text.index(";")]
    usernames_text = usernames_text[usernames_text.index(";") + 2 :]

    #writes captions to captions.txt
    captions_file.write("\n" + current_username + "\n")
    for caption in get_profile_captions(current_username):
        captions_file.write(caption + "\n")

    #writes urls to urls.txt
    image_urls_file.write("\n" + current_username + "\n")
    for url in get_profile_picture_urls(current_username):
        image_urls_file.write(url + "\n")
