# -*- coding: utf-8 -*-
#Simple dialog
#Copyright (C) 2020 guredora <contact@guredora.com>

import ctypes
def dialog(title,message):
	ctypes.windll.user32.MessageBoxW(0,message,title,0x00000040)
