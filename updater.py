# アップデーター
# Copyright (C) 2020 guredora <contact@guredora.com>
import os
import pathlib
import argparse
import requests
import subprocess
from simpleDialog import *
update_url="https://www.hogehoge.com/software/update.php"#アップデート確認用のURL
password=""#パスワードを設定。
parser = argparse.ArgumentParser(add_help=False)
parser.add_argument("arg1")
parser.add_argument("arg2")#アップデート確認の時はソフトのバージョンをアップデートするときはダウンロードurlを指定。
parser.add_argument("password")#passwordに設定してある文字列を指定。これが設定した物と違うと動かない。
parser.add_argument("-u", action="store_true")# アップデート実行のフラグ。実際にアップデートを行う時は指定。
args = parser.parse_args()
if args.password == password:
	if args.u:
		response = requests.get(args.arg2)
		up_name = os.path.basename(args.arg2)
		up_path = pathlib.Path(up_name)
		up_path.write_bytes(response.content)
		proc = subprocess.run((up_name, "-y"), stdout=subprocess.PIPE, stderr=subprocess.STDOUT, text=True)
		up_path.unlink()
		dialog("アップデートが正常に完了しました。アプリケーションを起動します。", "完了")
		subprocess.run((args.arg1))
	elif not args.u:
		url = "%s?name=%s&version=%s" % (update_url, args.arg1, args.arg2)
		response = requests.get(url)
		print(response.text)
