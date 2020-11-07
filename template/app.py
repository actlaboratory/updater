# -*- coding: utf-8 -*-
# Application Main

import proxyUtil
import AppBase
from views import main
import sys
import locale
import _locale
import update
import constants
import errorCodes
from simpleDialog import *
import os
import globalVars

class Main(AppBase.MainBase):
	def __init__(self):
		super().__init__()

	def initialize(self):
		"""アプリを初期化する。"""
		# プロキシの設定を適用
		if self.config.getboolean("network", "auto_proxy"):
			self.proxyEnviron = proxyUtil.virtualProxyEnviron()
			self.proxyEnviron.set_environ()
		else:
			self.proxyEnviron = None
		self.setGlobalVars()
		# update関係を準備
		if self.config.getboolean("general", "update"):
			globalVars.update.update(True)
		# メインビューを表示
		self.hMainView=main.MainView()
		if self.config.getboolean(self.hMainView.identifier,"maximized",False):
			self.hMainView.hFrame.Maximize()
		self.hMainView.Show()
		return True

	def setGlobalVars(self):
		globalVars.update = update.update()
		return

	def OnExit(self):
		#設定の保存やリソースの開放など、終了前に行いたい処理があれば記述できる
		#ビューへのアクセスや終了の抑制はできないので注意。
		# プロキシの設定を元に戻す
		if self.proxyEnviron != None: self.proxyEnviron.unset_environ()
		
		#戻り値は無視される
		return 0
