# ux - Micro Xylph

ux は軽量でシンプルな動作を目標としたソフトウェアシンセサイザです。C# で作られており、Mono 上でも動作します。


## 概要

ux は [Xylph](//www.johokagekkan.go.jp/2011/u-20/xylph.html) (シルフ) の後継として開発されています。Xylph の開発で得られた最低限必要な機能を絞り、なおかつ Xylph よりも軽快に動作するよう設計されています。C# で記述しつつ、極力高速な動作が目標です。

ux は モノフォニック、複数パート、ポルタメント、ビブラートなどの機能を持ち、音源として矩形波、16 段三角波、ユーザ波形、線形帰還シフトレジスタによる擬似ノイズ、4 オペレータ FM 音源を搭載しています。

現在 Wiki を構築中です。ハンドルの詳細など仕様については Wiki を参照してください: https://github.com/nanase/ux/wiki


## TODO in v0.2-dev

* プロジェクト共通
  - [x] #region の命名を変更

* ux (uxCore)
  - [x] 処理の最適化（特に複数ハンドルのキューイング）

* uxMidi
  - [x] ポリフォニック演奏への暫定対応

* uxPlayer
  - [ ] MIDI 接続の Linux 対応 (デバイスファイルからの読み取り)
  - [ ] WAVE 出力機能の実装
  - [ ] 任意の音源定義XMLファイルの指定


## 姉妹リポジトリ

* 


## 備考

* _ux_ と表記して _Micro Xylph (マイクロシルフ)_ と呼称し、プロジェクト内での表記も `ux` です(TeX のようなものです)。
* 性能を重視するためモノフォニック実装(1パート1音)です。ただし uxMidi でのドラムパートのみ 8 音のポリフォニックです。
* この仕様により大抵の MIDI ファイルは正常に再生できません。特に和音を持っている部分は音が抜けます。
* 音色がとにかく_貧弱_です。これは音源定義XMLファイルに充分な定義が無いためです。
  - リポジトリ内の以下のファイルが音源定義XMLファイルです。
    + [nanase/ux/uxConsole/ux_preset.xml](//github.com/nanase/ux/blob/v0.2-dev/uxConsole/ux_preset.xml)
    + [nanase/ux/uxPlayer/ux_preset.xml](//github.com/nanase/ux/blob/v0.2-dev/uxPlayer/ux_preset.xml)
  - 最新の定義ファイルを Gist に置いています: [gist.github.com/nanase/6068233](//gist.github.com/nanase/6068233)


## 動作確認
* Mono 2.10.8.1 (Linux Mint 14 64 bit)
* .NET Framework 4.5 (Windows 7 64 bit)
* (内部プロジェクトは互換性を理由に .NET Framework 4.0 をターゲットにしています)


## 使用素材
uxPlayer にて [p.yusukekamiyamane.com](http://p.yusukekamiyamane.com/) の Fugue Icons を使用しています。

(C) 2013 Yusuke Kamiyamane. All rights reserved.

These icons are licensed under a Creative Commons
Attribution 3.0 License.
<http://creativecommons.org/licenses/by/3.0/>

## ライセンス
Copyright &copy; 2013 Tomona Nanase.

MIT ライセンス
