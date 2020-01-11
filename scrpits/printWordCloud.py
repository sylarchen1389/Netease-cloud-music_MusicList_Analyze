import os,re  # 正则表达式库
import collections  # 词频统计库
import numpy as np  # numpy数据处理库
import jieba  # 结巴分词
import wordcloud  # 词云展示库
from PIL import Image  # 图像处理库
import matplotlib.pyplot as plt  # 图像展示库
import pandas as pd 

dir_path = ".\\assert\\lyrics\\"
input_img_dir_path = ".\\assert\\source_img\\"
ouput_img_dir_path = ".\\assert\\output_img\\"

if not os.path.exists(dir_path):
    os.mkdir(dir_path)


if not os.path.exists(input_img_dir_path):
    os.mkdir(input_img_dir_path)


if not os.path.exists(ouput_img_dir_path):
    os.mkdir(ouput_img_dir_path)

# 读取文件
lyrics_fn = open(dir_path+'lyrics.txt','r',encoding='UTF-8')
trashWord_fn =  open(dir_path+'Trash.txt','r',encoding='UTF-8')
param_fn = open(dir_path+'param.txt','r',encoding='UTF-8')

lyrics_data = lyrics_fn.read()
trashWord_data = trashWord_fn.read()

param = []
temp = 0
img_path = param_fn.readline()
pattern = re.compile('\n|\r|\t|\xa0| |')
img_path = re.sub(pattern,'',img_path)
for i in range(0,3):
    param.append(int(param_fn.readline()))

img_path = input_img_dir_path + img_path

lyrics_fn.close()
trashWord_fn.close()
param_fn.close()

# 文本预处理
pattern = re.compile(u'\t|\n|\.|-|:|;|\)|\(|\?|"|\xa0')  # 定义正则表达式匹配模式
lyrics_data = re.sub(pattern, '', lyrics_data)  # 将符合模式的字符去除

pattern = re.compile(u'\t|\n|\.|-|:|;|\)|\(|\?|"')  # 定义正则表达式匹配模式
trashWord_data = re.sub(pattern, '', trashWord_data)  # 将符合模式的字符去除

# 文本分词
lyrics = jieba.cut(lyrics_data, cut_all=False)  # 精确模式分词
trashWords = jieba.cut(trashWord_data, cut_all=False)
trash = []
print('!!!!!!!!!!!!!!!!!!!')
for word in trashWords:
    trash.append(word)
print('!!!!!!!!!!!!!!!!')

word_list = []
for word in lyrics:  # 循环读出每个分词
    if word not in trash:  # 如果不在去除词库中
        word_list.append(word)  # 分词追加到列表





# 词频统计
word_counts = collections.Counter(word_list)  # 对分词做词频统计
word_counts_top10 = word_counts.most_common(10)  # 获取前10最高频的词
print(word_counts_top10)  # 输出检查

# 词频展示
mask = np.array(Image.open(img_path))  # 定义词频背景
wc = wordcloud.WordCloud(
    font_path='C:/Windows/Fonts/simhei.ttf',  # 设置字体格式
    background_color='white',
    mask=mask,  # 设置背景图
    max_words=param[0],  # 最多显示词数
    max_font_size=param[1],  # 字体最大值
    scale=param[2]
)

wc.generate_from_frequencies(word_counts)  # 从字典生成词云
image_colors = wordcloud.ImageColorGenerator(mask)  # 从背景图建立颜色方案
wc.recolor(color_func=image_colors)  # 将词云颜色设置为背景图方案
plt.imshow(wc)  # 显示词云
plt.axis('off')  # 关闭坐标轴
print("Proccessing...please don't close the windows...")
plt.savefig(ouput_img_dir_path+"wordCloud.png")
print("WordCloud Print Successfully!")

