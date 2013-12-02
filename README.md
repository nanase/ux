# ux - Micro Xylph

ux は軽量でシンプルな動作を目標としたソフトウェアシンセサイザです。C# で作られており、Mono 上でも動作します。


## 概要

ux は Xylph (シルフ) の後継として開発されています。Xylph の開発で得られた最低限必要な機能を絞り、なおかつ Xylph よりも軽快に動作するよう設計されています。

ux は モノフォニック、複数パート、ポルタメント、ビブラートなどの機能を持ち、音源として矩形波、16 段三角波、ユーザ波形、線形帰還シフトレジスタによる擬似ノイズ、4 オペレータ FM 音源を搭載しています。

性能を重視するためモノフォニック実装(1パート1音)です。C# で記述しつつなるべく高速な動作が目標です。

現在 Wiki を構築中です。ハンドルの詳細など仕様については Wiki を参照してください: https://github.com/nanase/ux/wiki


## TODO in v0.2-dev

- [ ] ux - #region の命名を変更
- [ ] ux - 処理の最適化
- [ ] uxPlayer - MIDI 接続の Linux 対応 (デバイスファイルからの読み取り)
- [ ] uxPlayer - WAVE 出力機能の実装


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
