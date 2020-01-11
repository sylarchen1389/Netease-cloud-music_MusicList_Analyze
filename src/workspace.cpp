#include "..\include\workspace.h"
#include <Windows.h>
#include <codecvt>
using namespace std;

Workspace::Workspace(){
    musicList.reserve(500);
    INPUT_PATH = "assert/lyrics/lyrics.txt";
    IGNORE_PATH = "assert/lyrics/Trash.txt";
    WORD_OUTPUT_PATH = "assert/output/word_output.txt";
    MUSIC_OUTPUT_PATH = "assert/output/music_output.txt";
    INFO_PATH = "assert/output/info_output.txt";
    SEARCH_PATH = "assert/output/search_output.txt";

    
    //对要忽略的单词建立Trie树
    ifstream ignoreFile;
    ignoreFile.open(IGNORE_PATH, ios::in);
    if (!ignoreFile) {
        cout << "文件不存在\n";
        return;
    }
    else {
        cout << "打开成功\n";
    }

    while (!ignoreFile.eof()) {
        string buffer;
        string word;
        getline(ignoreFile, buffer, ' ');
        for (int i = 0; i < buffer.size(); ++i) {
            char ch = buffer[i];
            //保留小写
            if ((ch >= 'a') && (ch <= 'z')) {
                word.push_back(ch);
            }
            else if ((ch >= 'A') && (ch <= 'Z')) {
                word.push_back(ch + 32);
            }
            else {
                break;
            }
        }
        if (word.size() >= 1) {
            ignoreTree.addNode(word);
            //cout << word << endl;
        }
    }
}

Workspace::~Workspace(){
}

//流程控制
void Workspace::run(string target) {
    if (!input())
        return;
    if (!target.empty())
        cout << "要查找的单词的词频为：" << T.searchNode(target) << endl;
    printf("--------------------\n");
    T.preOrder();
    output(target);
    //T.printHotWords();
}

//从输入文件分离出单词、曲目名信息，并建立Trie树
bool Workspace::input() {
    ifstream inFile;
    inFile.open(INPUT_PATH, ios::in);
  
    if (!inFile) {
        cout << "文件不存在\n";
        return false;
    }
    else {
        cout << "打开成功\n";
    }
  
    bool flag = false;
    char ch;    
    char buffer[MAX_BUFFER_LEN];
    string musicName;       
    musicName.reserve(100);
    musicName.clear();
    //getline读取文件第一行有bug！！！！！
    //处理一下
    //getline(inFile, musicName, '\n');
    //cout << musicName << endl;
    //while (!((musicName.front() >= 'a' && musicName.front() <= 'z')
    //    || (musicName.front() >= 'A' && musicName.front() <= 'Z')))
    //    musicName.erase(musicName.begin());
    //if (!musicName.empty())
    //    musicList.emplace_back(musicName);

    while (!inFile.eof()) {
        
        inFile.getline(buffer, MAX_BUFFER_LEN, '\n');
        //cout << buffer << endl;
        size_t len = strlen(buffer);
        //cout << buffer << endl;
        //读取曲目名        
        if (buffer[10] == '*' && buffer[15] == '*') {
            getline(inFile, musicName, '\n');
            if (!musicName.empty() && musicName.size() > 4)
                musicList.emplace_back(musicName);
            musicName.clear();
            continue;
        }
        
        string word;
        for (int i = 0; i < len; ++i) {
            ch = buffer[i];
            //超出单词长度上限不再记录，直到遇到下一个分隔符
            if (flag &&
                (((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))))
                continue;
            flag = false;
            //保留小写
            if ((ch >= 'a') && (ch <= 'z')) {
                word.push_back(ch);
            }
            else if ((ch >= 'A') && (ch <= 'Z')) {
                word.push_back(ch + 32);
            }
            else{   
                if (word.size() >= MIN_WORD_LEN) {                        
                    //先判断是否要忽略
                    if (ignoreTree.searchNode(word) == 0 && !(word[0]=='b' && word[1] == 'y')) {                         
                        //cout << musicList.size() << endl;
                        T.addNode(word, musicList.size() - 1);
                    }                        
                }
                word.clear();                                
            }
            //当前单词已超过指定长度，超出部分忽略
            if (word.size() >= MAX_WORD_LEN)
                flag = true;
        }

        if (word.size() >= MIN_WORD_LEN) {            //先判断是否要忽略
            //新单词则更新的频率和曲目，否则只更新频率
            if (ignoreTree.searchNode(word) == 0 && !(word[0] == 'b' && word[1] == 'y')) {
                T.addNode(word, musicList.size() - 1);
            }
        }
    }
    inFile.close();
    return true;
}

void Workspace::output(string target) {
    ofstream output;

    //输出曲目列表
    output.open(MUSIC_OUTPUT_PATH, ios::out);
    for (vector<string>::iterator it = musicList.begin(); it != musicList.end(); it++)
        output << *it;
    output.close();

    //输出单词及曲目列表

    output.open(WORD_OUTPUT_PATH, ios::out);         
    for (auto it : T.hotwordList) {
        output << it.frequency << endl;
        output << it.word << endl;
        for (vector<AppearedMusic>::iterator i = it.appearedMusics.begin(); i != it.appearedMusics.end(); ++i) {
            output << i->count << "\\" << musicList.at(i->index);
            
        }
    }
    output.close();

    //输出统计信息
    output.open(INFO_PATH, ios::out);
    output << T.getWordSize() << endl;
    output << T.hotwordList.size() << endl;
    output << musicList.size() << endl;
    output << ignoreTree.getWordSize() << endl;
    output.close();

    //输出查询信息
    output.open(SEARCH_PATH, ios::out);
    WordNode* temp = T.searchNode(target);
    if (temp != nullptr) {
        output << temp->count << endl;
        output << temp->appeared.size() << endl;
        for (vector<AppearedMusic>::iterator i = temp->appeared.begin(); i != temp->appeared.end(); ++i) {
            output << i->count << "\\" << musicList.at(i->index);
        }
    }
    else {
        output << "\"" << target << "\"" << " does not exist in these musics." << endl;
    }
    output.close();
}
