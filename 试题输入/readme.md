##试卷读入工具

将试卷（word文档）输入到数据库中。

客户端的AS3对Html的支持不算很好，手机端就更水了。

但还是决定实现题目的图文混排就用html了。于是被告知了有将word转成html的方法还很好心地连c#代码都找给我了~~虽然word自带另存为html功能~~。

但是看一下word文档参差不齐的格式总觉得不靠谱便拖延症发作跑去学数据库了，直到那一天...

~~那一天，人类终于回想起了，曾经一度将几个字变成几KB的恐怖，还有被格式标签晃瞎的那份耻辱。~~

将试卷另存html之后格式标签多到连题目和选项在哪里都看不到了。

之后用DreamWeaver 8将**<*p*/>和<*img*/>之外的全部标签**都删除掉，虽然搞得上标下标之类的格式不见了，但是Flex都不怎么支持所以我没有一丝踌躇地~~减少了工作量~~按下确定键。

这之后就是用c#扫描题目和答案，整理好存入数据库的事了。

最主要用的2条正则，分别用来扫描题号和答案：
```
protected  Regex qNum=new Regex("(<p>\\d+[.．、])");
protected  Regex qAns=new Regex("[(（][  ]+[A-D,a-d][  ]+[)）]");
```
*关键是要注意包含中英文字符*

扫描题干代码：
```
Match match1 = qNum.Match(str);
Match match2 = match1.NextMatch();
while( match1.Success ){
    if(!match2.Success) question = str.Substring(match1.Index);
	else question = str.Substring(match1.Index , match2.Index-match1.Index);
    question = scanAnswer(question , paper.answer);
	question = question.Replace(match1.ToString() , "<p>");
    ...
    	match1 = match2;
	match2 = match2.NextMatch();
}
```

扫描答案代码：
```
String tmp1 = qAns.Replace(text , "( )");
String tmp2 = qAns.Match(text).ToString();
Switch(tmp2){
   ...
}
```

---
工具做的比较随便。但是折腾了一下没怎么用过的正则表达式也算有收获。

![](https://raw.githubusercontent.com/CloudTsang/OnlineTest/master/picture/test3.png)

