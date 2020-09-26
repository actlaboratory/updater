# -*- coding: utf-8 -*-
#app build tool
#Copyright (C) 2019 Yukio Nozawa <personal@nyanchangames.com>
import os
import sys
import subprocess
import shutil
import distutils.dir_util

def runcmd(cmd):
	proc=subprocess.Popen(cmd.split(), shell=True, stdout=1, stderr=2)
	proc.communicate()


if os.path.exists("dist\\updater.exe"):
	print("Clearling previous build...")
	shutil.rmtree("dist\\")

print("Building...")
runcmd("pyinstaller --onefile --log-level=ERROR updater.py constants.py")
print("Done!")
