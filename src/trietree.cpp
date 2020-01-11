#include "..\include\trietree.h"
using namespace std;

bool hotWordRule(HotWord w1, HotWord w2) {
    if (w1.frequency > w2.frequency)
        return true;
    else    
        return false;    
}

bool appearedMusicRule(AppearedMusic x, AppearedMusic y) {
    if (x.count > y.count)
        return true;
    else
        return false;
}

TrieTree::TrieTree() {
    root = new WordNode();
    hotwordList.reserve(HOTWORDLIST_SIZE * 100);
}

TrieTree::~TrieTree() {
    delete root;
}

//��ӵ��ʽ��
void TrieTree::addNode(string word) {
    WordNode *p = root;
    int index;
    for (int i = 0; i < word.length(); ++i) {
        index = word[i] - 'a';
        if (p->next[index] == nullptr) {
            p->next[index] = new WordNode();
        }
        p = p->next[index];
    }
    p->count++;
    if (!p->isWord) 
    { 
        p->isWord = true; 
    }
    wordSize++;
}

//��ӵ��ʽ���Ҹ�����Ŀ������
void TrieTree::addNode(string word, int number) {   
    if (number < 0)
        number = 0;
    WordNode *p = root;
    int index;
    for (int i = 0; i < word.length(); ++i) {
        index = word[i] - 'a';
        if (p->next[index] == nullptr) {
            p->next[index] = new WordNode();
        }
        p = p->next[index];
    }
    p->count++;    
    bool flag = false;
    for (vector<AppearedMusic>::iterator i = p->appeared.begin(); i != p->appeared.end(); ++i)
        if (i->index == number) {
            i->count++;            
            flag = true;
            break;
        }
    if (!flag) {
        AppearedMusic temp(number);
        p->appeared.emplace_back(temp);
    }
    p->isWord = true;
    wordSize++;
}

//ɾ������
void TrieTree::deleteNode(std::string word) {
    WordNode *temp = searchNode(word);
    if (temp == nullptr)
        return;
    temp->appeared.clear();
    temp->count = 0;
    temp->isWord = false;
    wordSize--;
}

//�鵽���ʣ����ش�Ƶ���޸õ����򷵻�0
WordNode* TrieTree::searchNode(string word) {
    WordNode *p = root;
    int index;
    for (int i = 0; i < word.length(); ++i) {
        index = word[i] - 'a';
        if (p->next[index] == nullptr) {
            return nullptr;
        }
        p = p->next[index];
    }
    if (word.length() < 1)
        p = nullptr;
    if (p!=nullptr && !p->isWord)
        p = nullptr;
    return p;
}

//ǰ��������ɰ��ֵ������������
//ͬʱ��¼���е���
void TrieTree::_preOrder(WordNode *head, string &word) {
    if (head == nullptr)
        return;
    if (head->isWord) {
        HotWord tempword(word, head->count);
        sort(head->appeared.begin(), head->appeared.end(), appearedMusicRule);

        tempword.setMusicList(head->appeared);
        hotwordList.emplace_back(tempword);
        //cout << word << "    " << head->count<<endl;
        //printf("%s%5d\n", word, head->count);     
    }
    for (int i = 0; i < TREE_SIZE; ++i)
        if (head->next[i] != nullptr) {
            word.push_back(i + 'a');
            _preOrder(head->next[i], word);
            word.pop_back();
        }
}

//ǰ���������ں���
void TrieTree::preOrder() {
    hotwordList.clear();
    string word;    
    _preOrder(root, word);
    sort(hotwordList.begin(), hotwordList.end(), hotWordRule);
}

//
//void TrieTree::_clear(WordNode *head) {
//    for (int i = 0; i < TREE_SIZE; ++i)
//        if (head->next[i] != nullptr) 
//            _clear(head->next[i]);                  
//    delete head;
//}
//
//void TrieTree::clear() {
//    for (int i = 0; i < TREE_SIZE; ++i)
//        if (root->next[i] != nullptr) 
//            _clear(root->next[i]);            
//}

//��ӡ��Ƶ����
void TrieTree::printHotWords() {
    for (int i = 0; i < hotwordList.size(); ++i) {
        HotWord temp = hotwordList[i];
        //cout<<temp.word<<"     "<<temp.frequency<<endl;
        //cout << "������Ŀ��\n";
        for (size_t j = 0; j < temp.appearedMusics.size(); ++j) {
            //cout << temp.musicList[j] << endl;
        }
    }
}

int TrieTree::getWordSize() {
    return wordSize;
}