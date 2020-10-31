*/
actlab updater
Copyright (C) 2020 guredora <contact@guredora.com>
/*

using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Diagnostics;

class program{
	//updater起動時に指定するwakeWord
	public const string SETTING_WAKEWORD = "hello";
	public static int Main(string[] args){
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
		using (Process application = Process.GetProcessById(pid)){//起動元プロセスの取得
			application.WaitForExit();//終了まで待機
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
			//zipを展開する処理
		}
		MessageBox.Show("アップデートが完了しました。アプリケーションを起動します。", "updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
		System.Diagnostics.Process.Start(application_name);//アプリケーション起動
		File.Delete(updater_file);//updaterの削除
		return 0;//正常終了
	}
}
