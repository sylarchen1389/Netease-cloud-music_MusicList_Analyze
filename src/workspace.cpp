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

    
    //��Ҫ���Եĵ��ʽ���Trie��
    ifstream ignoreFile;
    ignoreFile.open(IGNORE_PATH, ios::in);
    if (!ignoreFile) {
        cout << "�ļ�������\n";
        return;
    }
    else {
        cout << "�򿪳ɹ�\n";
    }

    while (!ignoreFile.eof()) {
        string buffer;
        string word;
        getline(ignoreFile, buffer, ' ');
        for (int i = 0; i < buffer.size(); ++i) {
            char ch = buffer[i];
            //����Сд
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

//���̿���
void Workspace::run(string target) {
    if (!input())
        return;
    if (!target.empty())
        cout << "Ҫ���ҵĵ��ʵĴ�ƵΪ��" << T.searchNode(target) << endl;
    printf("--------------------\n");
    T.preOrder();
    output(target);
    //T.printHotWords();
}

//�������ļ���������ʡ���Ŀ����Ϣ��������Trie��
bool Workspace::input() {
    ifstream inFile;
    inFile.open(INPUT_PATH, ios::in);
  
    if (!inFile) {
        cout << "�ļ�������\n";
        return false;
    }
    else {
        cout << "�򿪳ɹ�\n";
    }
  
    bool flag = false;
    char ch;    
    char buffer[MAX_BUFFER_LEN];
    string musicName;       
    musicName.reserve(100);
    musicName.clear();
    //getline��ȡ�ļ���һ����bug����������
    //����һ��
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
        //��ȡ��Ŀ��        
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
            //�������ʳ������޲��ټ�¼��ֱ��������һ���ָ���
            if (flag &&
                (((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))))
                continue;
            flag = false;
            //����Сд
            if ((ch >= 'a') && (ch <= 'z')) {
                word.push_back(ch);
            }
            else if ((ch >= 'A') && (ch <= 'Z')) {
                word.push_back(ch + 32);
            }
            else{   
                if (word.size() >= MIN_WORD_LEN) {                        
                    //���ж��Ƿ�Ҫ����
                    if (ignoreTree.searchNode(word) == 0 && !(word[0]=='b' && word[1] == 'y')) {                         
                        //cout << musicList.size() << endl;
                        T.addNode(word, musicList.size() - 1);
                    }                        
                }
                word.clear();                                
            }
            //��ǰ�����ѳ���ָ�����ȣ��������ֺ���
            if (word.size() >= MAX_WORD_LEN)
                flag = true;
        }

        if (word.size() >= MIN_WORD_LEN) {            //���ж��Ƿ�Ҫ����
            //�µ�������µ�Ƶ�ʺ���Ŀ������ֻ����Ƶ��
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

    //�����Ŀ�б�
    output.open(MUSIC_OUTPUT_PATH, ios::out);
    for (vector<string>::iterator it = musicList.begin(); it != musicList.end(); it++)
        output << *it;
    output.close();

    //������ʼ���Ŀ�б�

    output.open(WORD_OUTPUT_PATH, ios::out);         
    for (auto it : T.hotwordList) {
        output << it.frequency << endl;
        output << it.word << endl;
        for (vector<AppearedMusic>::iterator i = it.appearedMusics.begin(); i != it.appearedMusics.end(); ++i) {
            output << i->count << "\\" << musicList.at(i->index);
            
        }
    }
    output.close();

    //���ͳ����Ϣ
    output.open(INFO_PATH, ios::out);
    output << T.getWordSize() << endl;
    output << T.hotwordList.size() << endl;
    output << musicList.size() << endl;
    output << ignoreTree.getWordSize() << endl;
    output.close();

    //�����ѯ��Ϣ
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
