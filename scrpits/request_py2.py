#!/c/Users/Sylar/scoop/apps/python/current/python python 
import json
import requests

import urllib
from lxml import etree 

import io 
import os,sys,re
from lxml import etree
from tqdm import tqdm



def get_songs(url):

    headers = {
        'Referer' :'http://music.163.com',
        "Host":"music.163.com",
        "User-Agent":'Chrome/10'
    }

    res = requests.request('GET', url, headers=headers)
    html = etree.HTML(res.text)

    href_xpath = "//*[@id='song-list-pre-cache']//a/@href"
    name_xpath = "//*[@id='song-list-pre-cache']//a/text()"
    hrefs = html.xpath(href_xpath)
    names = html.xpath(name_xpath)
    
    song_ids = []
    song_names = []
    
    for href, name in zip(hrefs, names):
        song_ids.append(href[9:])
        song_names.append(str(name))
        #print(href, ' ', name)

    #print(html)
 
    return song_ids,song_names

def get_song_lyric(lyric_url):    
    
    headers = {
        'Referer' :'http://music.163.com',
        "Host":"music.163.com",
        "User-Agent":'Chrome/10'
    }

    res = requests.request('GET', lyric_url,headers=headers)
    if 'lrc' in res.json():
        #print(res)
        #print(res.json())
        #print(res.json()['lrc'])
        lyric = res.json()['lrc']['lyric']
        new_lyric = re.sub(r'[\d:.[\]]','',lyric)
        return new_lyric
    else:
        return ''

def get_all_song_lyric(lyric_url):

    all_word = '**********************************************' + os.linesep
    all_word.encode('utf-8')
    song_ids,song_names = get_songs(url=lyric_url)
    for (song_id, song_name) in tqdm(zip(song_ids, song_names)):
        lyric_url = 'http://music.163.com/api/song/lyric?os=pc&id=' + song_id + '&lv=-1&kv=-1&tv=-1'
        lyric = get_song_lyric(lyric_url)
        all_word = all_word + str(song_name) +  os.linesep + lyric 
        #print(str(song_name))
        all_word = all_word + '**********************************************' + os.linesep
    all_word.encode('utf-8')
    
    return all_word


def save_lyrics(url,dir_path):
    try:
        lyrics = get_all_song_lyric(lyric_url= url)

        pat = re.compile(r'\[.*\]')
        lrc = re.sub(pat, "", lyrics)
        lrc = lrc.strip()

        if not os.path.exists(dir_path):
            os.mkidr(dir_path)

        output = open(dir_path+'lyrics.txt', 'w',encoding='utf-8')
        output.write(lrc)
        output.close( )
        return lrc
    except:
        return ''


dir_path = ".\\assert\\lyrics\\"
print("Begin to get the lyrics")
input = open(dir_path+'url.txt','r',encoding='utf-8')
url = input.readline()
print("playlist_url: "+url)
input.close()

print("Geting lyrics , please wait a monent and don't close the consle.")

#lyrics = save_lyrics(url,dir_path)
#print(lyrics)

save_lyrics(url,dir_path)
print("Get lyrics successfully!")
