#include <iostream>
#include <fstream>
#include <string.h>
#include <string>
#include <vector>
#include <map>
#include <iomanip>
using namespace std;
#define MAX_BUFFER_LEN 128
#define MAX_FILE_NUM 105

//对单词建立倒排索引表
void input(int fileIndex, int (&fileSize)[MAX_FILE_NUM], map<string, bool[MAX_FILE_NUM]> &wordlist){
    char buffer[MAX_BUFFER_LEN];    
    int i, len;
    char ch = '\0';    
    int index = 0;    
    map<string, bool[MAX_FILE_NUM]>::iterator it;
    while ((ch = getchar()) != '#'){        
        //按规则分词
        if ((ch >= 'a') && (ch <= 'z')){
            buffer[index++] = ch;
        }else if ((ch >= 'A') && (ch <= 'Z')){
            buffer[index++] = ch + 32;
        }else{
            if (index >= 3){
                if (index > 10)
                    buffer[10] = '\0';
                else
                    buffer[index] = '\0';   
                //在本文件中不能重复出现
                string tempBuffer = buffer;
                it = wordlist.find(tempBuffer);
                if (it == wordlist.end()){
                    fileSize[fileIndex]++;
                    // cout<<tempBuffer<<" ";
                }
                wordlist[tempBuffer][fileIndex] = true;                                                
            }
            index = 0;            
        } 
    }
    if (index >= 3){
        if (index > 10)
            buffer[10] = '\0';
        else
            buffer[index] = '\0';          
            string tempBuffer = buffer;
            it = templist.find(tempBuffer);
            if (it == templist.end()){
                fileSize[fileIndex]++;
                // cout<<tempBuffer<<" ";
            }
            wordlist[tempBuffer][fileIndex] = true;
            templist[tempBuffer][fileIndex] = true;      
        
    }   
}
int main(){
    int n;
    cin>>n;
    int i;
    map<string, bool[MAX_FILE_NUM]> wordlist[MAX_FILE_NUM];      
    int fileSize[MAX_FILE_NUM];
    memset(fileSize, 0, sizeof(int)*MAX_FILE_NUM);
    for (i = 1; i <= n; ++i){
        input(i, fileSize, wordlist[i]);
        // cout<<"该文件单词有："<<fileSize[i]<<endl;
    }
    // for (map<string, bool[MAX_FILE_NUM]>::iterator it = wordlist.begin(); it != wordlist.end(); ++it){
    //     string strIndex= it->first;        
    //     cout<<strIndex<<endl;
    //     for (int j = 1 ; j <= n; ++j)
    //         cout<<wordlist[strIndex][j]<<"    ";            
    //     cout<<endl;
    // }
    int m, f1, f2;
    cin>>m;
    for (i = 0; i < m; ++i){
        cin>>f1>>f2;
        int common = 0;
        for (map<string, bool[MAX_FILE_NUM]>::iterator it = wordlist.begin(); it != wordlist.end(); ++it){
            string strIndex= it->first;        
            // cout<<strIndex<<endl;
            //查询该单词是否对两个文件都有记录
            if (wordlist[strIndex][f1] && wordlist[strIndex][f2])
                common++;
        }
        // cout<<common<<endl;
        double ans = static_cast<double>(common) / (fileSize[f1] + fileSize[f2] - common) * 100;
        cout<<setprecision(1)<<setiosflags(ios::fixed)<<ans<<"%"<<endl;
    }
    // system("pause");
}