#include <ctime>
#include "..\include\workspace.h"

using namespace std;
Workspace wordspace;

int main(int argc, char *argv[]){
    //system("chcp 65001");
    cout << "参数数量：" << argc << endl;
    string target = "";
    if (argc > 1) {        
        target = argv[1];
        cout << "要查找的单词是" << target << endl;
    }
    clock_t begin = clock();
    wordspace.run(target);            
    clock_t end = clock();
    cout<<static_cast<double>(end - begin) / CLOCKS_PER_SEC * 1000<<"ms"<<endl;
    system("pause");
}