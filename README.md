SUDALV's changes: I've added Treasure Chests lists by location (sorry, only 2 locations have a names, but it was still helpful for me to find last 2 chests)


![DL Count](https://img.shields.io/github/downloads/turtle-insect/OctopathTraveler/total.svg)
[![Build status](https://ci.appveyor.com/api/projects/status/p0qp4jhksi2j0ktq?svg=true)](https://ci.appveyor.com/project/turtle-insect/octopathtraveler)

# 寄付
<a href="https://www.buymeacoffee.com/06yi7RLlT" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/black_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" ></a>

# Langage
[English](README_EN.md)

# 概要
Switch OCTOPATH TRAVELER(オクトパス トラベラー)のセーブデータ編集Tool

# ソフト
http://www.jp.square-enix.com/octopathtraveler/

# 実行に必要
* Windows マシン
* .NET Framework 4.7.1の導入
* セーブデータの吸い出し
* セーブデータの書き戻し

# Build環境
* Windows 10(64bit)
* Visual Studio 2017

# 編集時の手順
* saveDataを吸い出す
   * 結果、以下が取得可能
      * KSSaveData1(KSSaveData2、KSSaveData3、、、)
      * KSSystemData
* KSSaveData1(KSSaveData2、KSSaveData3、、、)を読み込む
* 任意の編集を行う
* KSSaveData1(KSSaveData2、KSSaveData3、、、)を書き出す
* saveDataを書き戻す

# Special Thanks
* https://gbatemp.net/threads/octopath-traveler-save-editing.511125/
   * [SleepyPrince](https://gbatemp.net/members/sleepyprince.94652/)
   * [Takumah](https://gbatemp.net/members/takumah.456165/)
   * [Translate English by gen212](https://github.com/gen212/OctopathTraveler)
