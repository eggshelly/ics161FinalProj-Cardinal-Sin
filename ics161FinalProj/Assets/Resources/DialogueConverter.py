#DialogueConverter.py
# -*- coding: utf-8 -*-

#Michaela Gonzales
#michacg2@uci.edu

"""
This file reads from a word document and creates a an csv file.
Will appropriately place dialogue lines into cell to use for UNITY. 

ASSUMPTIONS:
    -Dialogue.txt is in the same directory as this file.

"""
##HEADER INITIALIZATION##
import csv
import os

##VARIABLES##
FILENAME = "Dialogue.txt"

##SCRIPT##

def run(file):   
    canmakecsv = False
    text = list()

    for line in file:
        if line == "INTRODUCTION\n" or (" " in line and line[:line.index(" ")] in ["INTRODUCTION", "WINTER", "SPRING", "SUMMER", "AUTUMN"]): #means new wb!
            if canmakecsv:
                makecsv(title, text)
            canmakecsv = True

            title = line.rstrip()
            text  = list()

        elif line == "\n":
            pass

        else:
            if "," in line:
                line = line.replace(",","XYZ")
            text.append(line.rstrip())
            
    makecsv(title,text)


def makecsv(title:str, text:str):
    #folder = "Haruka\\" if "SPRING" in title else "Akiko\\" if "AUTUMN" in title else "Touka\\" if "WINTER" in title else "Natsuki\\" if "SUMMER" in title else ""
    #filePath = f"\CSV\{folder}{title}.csv"
    oldTitle = title
    if os.path.isfile(f"{title}.csv"):  #already in directory
        title += "_NEW"
        #filePath = f"\CSV{folder}{title}"


    with open(f"{title}.csv", "w") as csvfile:
        filewriter = csv.writer(csvfile, delimiter=',',
                            quotechar='|', quoting=csv.QUOTE_MINIMAL)

        filewriter.writerow(["SPEAKER", "CONVO", "SPRITE", "AUDIO", "BACKGROUND"])
        for line in text:
            print(line)
            speaker = line[:line.index(":")]
            convo   = line[line.index(":")+1:]
            sprite  = "none" if speaker == "MC" else "default"
            audio   = oldTitle[:oldTitle.index(" ")] if " " in oldTitle else oldTitle
            bg      = "BG "

            filewriter.writerow([speaker, convo, sprite, audio, bg])
    

def cleanup(file):
    file.close()
    
if __name__ == "__main__":
    file = open(FILENAME, "r")
    run(file)
    cleanup(file)
    print("DialogueConverter.py: OUTPUT END")
