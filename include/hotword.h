#pragma once
#include <string>
#include <vector>

class HotWord{
public:
    std::string word;
    int frequency;
    std::vector<std::string> musicList;
public:
    HotWord(){
        word = "\0";
        frequency = 0;      
    }

    HotWord(std::string word, int frequency) {
        this->word = word;
        this->frequency = frequency;        
    }

    ~HotWord() {
    }

    void setMusicList(std::vector<std::string> _musicList) {
        musicList.reserve(_musicList.size());
        musicList.assign(_musicList.begin(), _musicList.end());
    }

};