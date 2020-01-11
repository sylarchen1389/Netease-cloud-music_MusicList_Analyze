import os,sys,re

import re



remove_words = [u'的', u'，', u'和', u'是', u'随着', u'对于', u'对', u'等', u'能', u'都', u'。', u' ', u'、', u'中', u'在', u'了',
                u'通常', u'如果', u'我们', u'需要', 'the', 'you', 'me', 'your', 'with', 'on', u'歌词', '\'', 'up', 'just', 'our', 'my', 'without', 'oh', 'over', 'of', u'作词', u'作曲', 'a', 'than', 'be', 'and', 'in', 'to', 'for', 'it', 'or', '(', ')', '|', 'm', '\\', 'has', 'there', 'around', 'let', 'off', 'can', 'make', 'in', 'out', 'no', 'get', 'do', 'is', 'these', 'will', 'got', 'gotta', 'but', 'only', 'know', 'hear', 'at', 'come', 'could', 'too', 'cause', 'from', 'have', 'you', 'by', 'what', 'much', 'one', 'am', 'gotta', 'd', 'an', 'these', 'that', 'every', 's', 'not', 'like', 'yeah', 'things', 'were', 'they', 'you', 'are', '||', 'their', 'he', 'are', 'as', 'give', 'It', 'look', 'walk', 'set', 'a', 'been', 'f', 've', 're', 'nicht', 'tell', 'want', '...', 'round', 'how', 'should', 'if', 'going', 'about', 'da', 's', 'thing', '!', 'as', 'then', u'’', 'those', 'so', 'take', 'many', 'mich', 'all', 'ing', 'this', 'had', 'mel', 'l', 'used', 'who', 'es', 'das', 'You', 'us', 'go', 'man', 'My', 'All', 'run', 'ahead', 'coming', 'now', 'back', 'away', 'ich', 'dose', 'dosen', 'mir', 'good', 'made', 'them', 'his', 'here', '（', '）', 'seen', 'even', 'a', 'little', 'me', 'watch', 'step', 'ist', 'said', 'n', 'day', 'where', '&', 't', 'So', '\\xa0 ', 'don', ',', 'was', 'ä', 'll', 'we', 'see', 'when', 'gonna', 'call', 'into', 'bring', 'any', 'The', 'the', 'say', 'put', 'And', 'far', '\\', 'everything', 'when', 'makes', 'its', 'told', 'think', 'really', 'I', 'did', '/', 'me', 'la', 'why', 'more', 'du', 'hold', 'br', 'baby', 'To', 'Les', 'still', 'move', 'zu', 'We', 'youl', 'name', 'a', 'Cause', u'我', 'der', 'wanted', 'and', 'times', 'ma', 'den', 'wanna', 'hands', 'fee', 'need', 'you', 'through', 'find', 'would', 'i', 'ever', 'thought', 'we']  # 自定义去除词库



output = open('Trash.txt', 'w',encoding='utf-8')
for word in remove_words:
    output.write(word+" ")
output.close( )
           