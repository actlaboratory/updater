/*
actlab updater
Copyright (C) 2020 guredora <contact@guredora.com>

*/

using System;
using System.IO;
using Ionic.Zip;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Diagnostics;

class program{
	//updater起動時に指定するwakeWord
	public const string SETTING_WAKEWORD = "hello";
	public static int run(string[] args){
		if(args.Length < 5){
			return 0;//コマンドライン引数が足りない
		}
		string application_name = args[0];//起動元のプログラムファイル名
		string wakeWord = args[1];//wakeWord
		string updater_file = args[2];//ダウンロードされたupdaterのファイル名
		string updater_hash = args[3];//updaterのハッシュ
		int pid = int.Parse(args[4]);//起動元アプリケーションのPid
		if(wakeWord != SETTING_WAKEWORD){//wakeWordが正しくない
			MessageBox.Show("wakeWordが正しくありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return 1;//終了
		}
		try{
			using (Process application = Process.GetProcessById(pid)){
				application.WaitForExit();//呼び出し元アプリケーションの終了待機
			}
		}
		catch(ArgumentException e){
			Console.WriteLine(e.Message);
		}
		if(!File.Exists(updater_file)){//updaterが存在しない
			MessageBox.Show("指定されたアップデーターが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return 2;//終了
		}
		using (FileStream file = new FileStream(updater_file, FileMode.Open, FileAccess.Read)){
			byte[] sha1 = SHA1.Create().ComputeHash(file);//ハッシュの算出
			string hash = BitConverter.ToString(sha1).Replace("-", "").ToLower();
			if(updater_hash != hash){//ハッシュが違う
				MessageBox.Show("アップデーターのハッシュが指定されたものと一致しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return 3;//終了
			}
			file.Seek(0, SeekOrigin.Begin);//streamの開始位置をリセット
			using(ZipFile zip = ZipFile.Read(file, System.Text.Encoding.GetEncoding("shift_jis"))){//zipFileを開く
				zip.ExtractAll(".", ExtractExistingFileAction.OverwriteSilently);//展開
			}
		}
		MessageBox.Show("アップデートが完了しました。アプリケーションを起動します。", "updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
		System.Diagnostics.Process.Start(application_name);//アプリケーション起動
		File.Delete(updater_file);//updaterの削除
		return 0;//正常終了
	}

	public static int Main(string[] args){
		try{
			return run(args);
		}
		catch(Exception e){
			MessageBox.Show("エラーが発生しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			using (StreamWriter writer = new StreamWriter("updater_errorLog.txt", false, System.Text.Encoding.GetEncoding("shift_jis"))){
				writer.WriteLine(e.ToString());
			}
			return 1;
		}
	}
}
