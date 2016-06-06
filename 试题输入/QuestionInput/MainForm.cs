/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/4/18
 * 时间: 11:02
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace QuestionInput
{
	public partial class MainForm : Form
	{
		protected TestInfo paper  = new TestInfo();
		protected int opStage=0;
		protected  Regex qNum=new Regex("(<p>\\d+[.．、])");
		protected Regex qNum_Split = new Regex("\\d+");
		protected  Regex qAns=new Regex("[(（][  ]+[A-D,a-d][  ]+[)）]");
		protected  Regex qAns_Split=new Regex("[A-d,a-d]");
		public MainForm()
		{
			InitializeComponent();
			Tips.Text="请输入试题";
		}
		
		void BtnScanClick(object sender, EventArgs e)
		{
			if(opStage!=0 && opStage!=1){
				Tips.Text="请初始化！";
				return;
			}
			String str = this.txtInput.Text;
			int qno = 1;

			Match match1 = qNum.Match(str);
			Match match2 = match1.NextMatch();
			trace(match1.ToString());
			String showHtml="";
			while( match1.Success ){
				String question;
				if(!match2.Success) question = str.Substring(match1.Index);
				else question = str.Substring(match1.Index , match2.Index-match1.Index);
				
				#region 扫描题目与答案并储存
				question = scanAnswer(question , paper.answer);
				question = question.Replace(match1.ToString() , "<p>");
				showHtml+= ("<p/><p>第"+qno+"题 ： </p>");
				showHtml += question;
				trace("\n第"+qno+"题 ： ");
				trace(question);
				paper.question.Add(question);
				#endregion
				match1 = match2;
				match2 = match2.NextMatch();
				qno++;
			}
			txtHtml.DocumentText = showHtml;
			opStage = 1;
		}
		void BtnInitClick(object sender, EventArgs e)
		{
			Init();
			Tips.Text="已初始化";
		}
		
		void BtnCreateClick(object sender, EventArgs e)
		{
			if(opStage!=1){
				Tips.Text="请扫描试卷数据！";
				return;
			}
			if(txtName.Text=="") {
				Tips.Text="请输入试卷名称！";
				return;
			}
			else paper.name = txtName.Text;
			if(txtKey.Text=="" || isEnglish(txtKey.Text)) {
				Tips.Text="请输入试卷的英文名用作数据库key！";
				return;
			}
			else paper.dbkey = txtKey.Text;
			if(txtTime.Text==""){
				Tips.Text="请输入试卷的限制时间！";
				return ;
			}
			else paper.time =int.Parse( txtTime.Text);
			paper.description = txtDesc.Text;
			txtOutput.Text = paper.toString();
			opStage =2;
		}
		
		void BtnSaveClick(object sender, EventArgs e)
		{
			if(opStage !=2){
				Tips.Text="需要先生成试卷数据...";
				return;
			}
			paper.SaveTest();
			Init();
			Tips.Text="已将试卷储存至数据库。";
		}
		
		void trace(Object str){
			System.Diagnostics.Debug.WriteLine(str);
		}
		Boolean isEnglish(String str){
			Match mInfo = Regex.Match(str,@"[\u4e00-\u9fa5]");
			return mInfo.Success;
		}
		
		void Init(){
			paper = new TestInfo();
			txtName.Text="";
			txtKey.Text="";
			txtInput.Text="";
			txtOutput.Text="";
			txtTime.Text="";
			txtDesc.Text="";
			txtHtml.DocumentText="";
			opStage = 0;
		}
		
//		private const Regex REG = new Regex("/(\(\s+[A-D,a-d]\s+\))/");
		
		#region 答案扫描函数
		String scanAnswer(String text ,List<int> arr){
			String tmp1 = qAns.Replace(text , "( )");
			String tmp2 = qAns.Match(text).ToString();
			int ans=-1;
			switch (tmp2) {
				case "A":
				case "a":
					ans=0;
					break;
				case "B":
				case "b":
					ans=1;
					break;
				case "C":
				case "c":
					ans=2;
					break;
				case "D":
				case "d":
					ans=3;
					break;
			}
			arr.Add(ans);
			return tmp1;
		}
		#endregion
	}
}
