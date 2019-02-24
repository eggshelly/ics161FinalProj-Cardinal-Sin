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

##VARIABLES##
FILENAME = "Dialogue.txt"

##SCRIPT##

def run(file):   
    canmakecsv = False
    text = list()

    for line in file:
        if line == "INTRODUCTION\n" or (" " in line and line[:line.index(" ")] in ["INTRODUCTION", "WINTER", "SPRING", "SUMMER", "AUTUMN"]): #means new wb!
            print(line)
            if canmakecsv:
                makecsv(title, text)
            canmakecsv = True

            title = line.rstrip()
            text  = list()

        else:
            if "," in line:
                line = line.replace(",","XYZ")
            text.append(line.rstrip())
            
    makecsv(title,text)


def makecsv(title:str, text:str):
    with open(f"{title}.csv", "w") as csvfile:
        filewriter = csv.writer(csvfile, delimiter=',',
                            quotechar='|', quoting=csv.QUOTE_MINIMAL)

        filewriter.writerow(["SPEAKER", "CONVO", "SPRITE"])
        for line in text:
            print(line)
            filewriter.writerow([line[:line.index(":")], line[line.index(":")+1:], "default"])


def cleanup(file):
    file.close()
    
if __name__ == "__main__":
    file = open(FILENAME, "r")
    run(file)
    cleanup(file)
    print("DialogueConverter.py: OUTPUT END")
