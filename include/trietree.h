#pragma once
#include <iostream>
#include <string>
#include <algorithm>
#include <vector>
#include "hotword.h"

#define TREE_SIZE 26
#define MAX_WORD_LEN 20
#define MIN_WORD_LEN 3

//Trie���еĻ���Ԫ��Ϊ���ʽ��
class WordNode{
public:
    WordNode *next[TREE_SIZE];          //��̽��
    bool isWord;                        //����ý��ʱ�Ƿ���һ�������ĵ���
    //��isWord == true ʱ����������Բ�������
    int count;                          //��Ƶ    
    std::vector<std::string> musicList; //���ֵ���Ŀ��
public:
    WordNode(){
        count = 0;
        isWord = false;
        for (size_t i = 0; i < TREE_SIZE; ++i)
            next[i] = nullptr;
        musicList.reserve(50);
    }
};

//Trie��������ȫ��������Ϣ��Ҫ���Եĵ���
class TrieTree
{
private:
    int wordSize;                       //���ʵ�����
    WordNode *root;
    const int HOTWORDLIST_SIZE = 100;   //Ҫ��ӡ�ĸ�Ƶ��������
public:
    std::vector<HotWord> hotwordList;   //��Ƶ�����б�

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