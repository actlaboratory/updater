# アップデーター
# Copyright (C) 2020 guredora <contact@guredora.com>
import os
import argparse
import subprocess
import constants
from simpleDialog import *
import psutil
from time import sleep
import sys
import zipfile

def exchandler(type, exc, tb):
	dialog("エラーが発生しました")
	msg=traceback.format_exception(type, exc, tb)
	print("".join(msg))
	f=open("errorLog.txt", "a")
	f.writelines(msg)
	f.close()
sys.excepthook=exchandler

parser = argparse.ArgumentParser(add_help=False)
parser.add_argument("arg1")# アプリケーションのフルパスを指定
parser.add_argument("wakeWord")#constants.wakeWordに設定してある文字列を指定。これが設定した物と違うと動かない。
parser.add_argument("arg2")# アップデート用のzipファイルの場所を指定

args = parser.parse_args()
if args.wakeWord == constants.wakeWord:
	process_list = []
	for proc in psutil.process_iter():
		process_list.append(proc.name)
	if os.path.basename(args.arg1) in process_list:# アプリケーションの修了確認
		dialog("アップデートを行う前に%sを終了してください" % (os.path.basename(args.arg1)), "エラー")
		sys.exit()
	with zipfile.ZipFile(args.arg2) as up:
		up.extractall(os.getcwd())
	os.remove(args.arg2)					#アップデータを削除
	dialog("アップデートが正常に完了しました。アプリケーションを起動します。", "完了")
	subprocess.Popen((args.arg1))# アプリケーションを起動



