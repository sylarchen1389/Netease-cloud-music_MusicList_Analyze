# 导入扩展库
import re  # 正则表达式库
import collections  # 词频统计库
import numpy as np  # numpy数据处理库
import jieba  # 结巴分词
import wordcloud  # 词云展示库
from PIL import Image  # 图像处理库
import matplotlib.pyplot as plt  # 图像展示库

# 读取文件
fn = open(r'C:\Users\Sylar\Desktop\Code\metalLcWordCloud\lyrics.txt', 'r',
          encoding='UTF-8')  # 打开文件
string_data = fn.read()  # 读出整个文件
fn.close()  # 关闭文件

# 文本预处理
pattern = re.compile(u'\t|\n|\.|-|:|;|\)|\(|\?|"')  # 定义正则表达式匹配模式
string_data = re.sub(pattern, '', string_data)  # 将符合模式的字符去除

# 文本分词
seg_list_exact = jieba.cut(string_data, cut_all=False)  # 精确模式分词
object_list = []
remove_words = [u'的', u'，', u'和', u'是', u'随着', u'对于', u'对', u'等', u'能', u'都', u'。', u' ', u'、', u'中', u'在', u'了',
                u'通常', u'如果', u'我们', u'需要', 'the', 'you', 'me', 'your', 'with', 'on', u'歌词', '\'', 'up', 'just', 'our', 'my', 'without', 'oh', 'over', 'of', u'作词', u'作曲', 'a', 'than', 'be', 'and', 'in', 'to', 'for', 'it', 'or', '(', ')', '|', 'm', '\\', 'has', 'there', 'around', 'let', 'off', 'can', 'make', 'in', 'out', 'no', 'get', 'do', 'is', 'these', 'will', 'got', 'gotta', 'but', 'only', 'know', 'hear', 'at', 'come', 'could', 'too', 'cause', 'from', 'have', 'you', 'by', 'what', 'much', 'one', 'am', 'gotta', 'd', 'an', 'these', 'that', 'every', 's', 'not', 'like', 'yeah', 'things', 'were', 'they', 'you', 'are', '||', 'their', 'he', 'are', 'as', 'give', 'It', 'look', 'walk', 'set', 'a', 'been', 'f', 've', 're', 'nicht', 'tell', 'want', '...', 'round', 'how', 'should', 'if', 'going', 'about', 'da', 's', 'thing', '!', 'as', 'then', u'’', 'those', 'so', 'take', 'many', 'mich', 'all', 'ing', 'this', 'had', 'mel', 'l', 'used', 'who', 'es', 'das', 'You', 'us', 'go', 'man', 'My', 'All', 'run', 'ahead', 'coming', 'now', 'back', 'away', 'ich', 'dose', 'dosen', 'mir', 'good', 'made', 'them', 'his', 'here', '（', '）', 'seen', 'even', 'a', 'little', 'me', 'watch', 'step', 'ist', 'said', 'n', 'day', 'where', '&', 't', 'So', '\\xa0 ', 'don', ',', 'was', 'ä', 'll', 'we', 'see', 'when', 'gonna', 'call', 'into', 'bring', 'any', 'The', 'the', 'say', 'put', 'And', 'far', '\\', 'everything', 'when', 'makes', 'its', 'told', 'think', 'really', 'I', 'did', '/', 'me', 'la', 'why', 'more', 'du', 'hold', 'br', 'baby', 'To', 'Les', 'still', 'move', 'zu', 'We', 'youl', 'name', 'a', 'Cause', u'我', 'der', 'wanted', 'and', 'times', 'ma', 'den', 'wanna', 'hands', 'fee', 'need', 'you', 'through', 'find', 'would', 'i', 'ever', 'thought', 'we']  # 自定义去除词库

for word in seg_list_exact:  # 循环读出每个分词
    if word not in remove_words:  # 如果不在去除词库中
        object_list.append(word)  # 分词追加到列表

# 词频统计
word_counts = collections.Counter(object_list)  # 对分词做词频统计
word_counts_top10 = word_counts.most_common(10)  # 获取前10最高频的词
print(word_counts_top10)  # 输出检查

# 词频展示
mask = np.array(Image.open(
    r'C:\Users\Sylar\Desktop\Code\metalLcWordCloud\wordcloud.jpg'))  # 定义词频背景
wc = wordcloud.WordCloud(
    font_path='C:/Windows/Fonts/simhei.ttf',  # 设置字体格式
    background_color='white',
    mask=mask,  # 设置背景图
    max_words=225,  # 最多显示词数
    max_font_size=150,  # 字体最大值
    scale=32
)

wc.generate_from_frequencies(word_counts)  # 从字典生成词云
image_colors = wordcloud.ImageColorGenerator(mask)  # 从背景图建立颜色方案
wc.recolor(color_func=image_colors)  # 将词云颜色设置为背景图方案
plt.imshow(wc)  # 显示词云
plt.axis('off')  # 关闭坐标轴
plt.show()  # 显示图像
