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
    TrieTree T;             //全部单词的Trie树
    TrieTree ignoreTree;    //要忽略的单词
    std::vector<std::string> musicList;
    //文件路径
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

