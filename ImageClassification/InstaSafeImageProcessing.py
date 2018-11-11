import requests
import os

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
        'authorization': "Basic YWNjX2ZiNDMxNDBmYTliZjJiNTo0ZmNjMDRjYmE2YWZjNWU0MWNmMGYyYTc1NGZmMzJhNw=="
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
    return threat_level / 10

def assessImages(self):
    urls = open(self, "r").read().split(", ")
    for i in range (0, len(urls)):
        print(assessViolence(getRawImageData(urls[i])))

def getAccountData(self):
    file_input = open(self, "r").readlines()
    file_output = open("output.txt", "w")
    i = 0;
    while i < len(file_input):
        i += 1
        file_output.write(file_input[i])
        i += 1
        while len(file_input[i]) > 1:
            file_output.write(str(assessViolence(getRawImageData(file_input[i]))) + "\n")
            i += 1
            if i == len(file_input):
                file_output.close()
                return



#def writeAssesedViolenceLevels(self):
abspath = os.path.abspath(__file__)
dname = os.path.dirname(abspath)
os.chdir(dname)
getAccountData("instaImageURLs.txt");
#assessImages("C:\\Users\\risha\\Desktop\\InstaSafe\\imageURLs.txt")
#print(assessViolence(getRawImageData("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTqMbWLOnud6K5l0VGiUEh3zZLSIJm5rWN42OCkT_yvbgM76U4APQ")))
#print(assessViolence(getRawImageData("https://img.huffingtonpost.com/asset/59e0f9051500002b00da1585.jpg?ops=crop_677_0_1317_1211,scalefit_720_noupscale")))
#print(assessViolence(getRawImageData("https://s3.amazonaws.com/imagga-demo-uploads/tagging-demo/1d83238f02f934e71035d99f83dfbbd7.jpg")))
#print(getRawImageData("https://s3.amazonaws.com/imagga-demo-uploads/tagging-demo/1d83238f02f934e71035d99f83dfbbd7.jpg"))
