#pragma once
#include <iostream>
#include <string>
#include <algorithm>
#include <vector>
#include "hotword.h"

#define TREE_SIZE 26
#define MAX_WORD_LEN 20
#define MIN_WORD_LEN 3

//Trie树中的基本元素为单词结点
class WordNode{
public:
    WordNode *next[TREE_SIZE];          //后继结点
    bool isWord;                        //到达该结点时是否是一个完整的单词
    //当isWord == true 时，后面的属性才有意义
    int count;                          //词频    
    std::vector<std::string> musicList; //出现的曲目名
public:
    WordNode(){
        count = 0;
        isWord = false;
        for (size_t i = 0; i < TREE_SIZE; ++i)
            next[i] = nullptr;
        musicList.reserve(50);
    }
};

//Trie树，保存全部单词信息和要忽略的单词
class TrieTree
{
private:
    int wordSize;                       //单词的数量
    WordNode *root;
    const int HOTWORDLIST_SIZE = 100;   //要打印的高频单词数量
public:
    std::vector<HotWord> hotwordList;   //高频单词列表

public:
    TrieTree();
    ~TrieTree();
    int getWordSize();
    void addNode(std::string word);
    void addNode(std::string word, const std::string musicName);
    int searchNode(std::string word);
    void _preOrder(WordNode *head, std::string &word);
    void preOrder();
    /*void _clear(WordNode *head);
    void clear();*/
    void printHotWords();
};