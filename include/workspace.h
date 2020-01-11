#pragma once
#include "trietree.h"

#include <iostream>
#include <fstream>
#include <string.h>
#include <string>

#define MAX_BUFFER_LEN 1024

class Workspace
{
private:
    TrieTree T;             //ȫ�����ʵ�Trie��
    TrieTree ignoreTree;    //Ҫ���Եĵ���
    std::vector<std::string> musicList;
    //�ļ�·��
    std::string INPUT_PATH;
    std::string WORD_OUTPUT_PATH;
    std::string MUSIC_OUTPUT_PATH;
    std::string IGNORE_PATH;
public:
    Workspace();
    ~Workspace();
    void run(std::string target);
    bool input();
    void output(std::string target);
};

