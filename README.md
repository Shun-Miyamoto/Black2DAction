# 操作

移動[左右キー]

ジャンプ[Z]

近距離攻撃[X]

通常ダッシュ[C]

チャージダッシュ[D]長押し～離す

エネルギー弾[S]（左上の下のゲージが紫色のときのみ）

# アクション等の基本仕様

攻撃は近距離攻撃が基本

ダッシュ回避中に**あえて敵の攻撃に当たりにいく**ことにメリットがあるゲーム性

ダッシュ回避中に攻撃に当たるとMPがたまり、MPを消費して大技を放てる

## ドレインダッシュ（ダッシュ回避）

ドレインダッシュ（ダッシュ回避）で敵や一部の攻撃をすりぬけると**MP**がたまる（ドレイン）

MPを消費して**大技**（エネルギー弾とか）を放てる

**赤い攻撃**は通常のダッシュ回避では避けられず、ちゃんとかわす必要がある

## 一部ボスの討伐などで追加されるようにしたいアクション（今はアクションの解放部分を開発中なので最初からある）

### チャージダッシュ(今は最初からあるが、途中のボス討伐で解放されるようにしたい)
スクリプトだとSuperDashとか書いてある（ChargeDashとか書いてるとチャージ中なのかダッシュ中なのかどっちの話をしてるのかよくわからないから）

赤い攻撃も回避できる

通常の**2倍**の量のMP吸収できる（通常の攻撃もチャージダッシュで当たれば2倍）

通常のダッシュで回避可能な攻撃は**全て**チャージダッシュでも回避可能

# プログラミング等での重要事項

## ジャンプ

床にはタグ**Ground**をつける

Groundタグにプレイヤーの足元のコライダーが触れたかで着地判定を行う。Groundタグのついてない足場ではジャンプできない

## インターフェース

### ダメージ用インターフェース

CommonScript/Interface/IDamageable.cs

ダメージ量：Int value、攻撃の方向（ノックバックなどに使う）：Vector2 vector、ダメージの種類：Int type（ダッシュで避けられるかどうか）

プレイヤーが受けるダメージ:ダメージの種類typeは、0なら通常ダッシュで避けられる、1は赤攻撃用で、通常ダッシュでは避けられずチャージダッシュを使えば避けられる、2なら何があっても避けられない（現状敵の攻撃に使うつもりはなく、地形などのダメージを想定）

敵が受けるダメージ：

### ドレイン(ダッシュで当たるとMP吸収ができるもの)用インターフェース

CommonScript/Interface/IDrainable.cs

接触ダメージが無いものでもドレイン可能な敵を制作する事を想定して、ダメージとは別にインターフェースを作っている。

Drain()がtrueのものは通常ダッシュでもチャージダッシュでもドレイン可能

チャージダッシュでのみドレイン可能な赤い攻撃などは、SuperDrain()のみtrueでDrain()がfalseにする

### プレイヤーがダッシュでぶつかった相手に何かする用インターフェース

CommonScript/Interface/IDashHit.cs

プレイヤーが通常ダッシュでぶつかった相手にはこのインターフェースのNormalDashHit()が、チャージダッシュでぶつかった相手にはISuperDash()が呼び出される。

## ファイル等の位置

各ボスのプレファブやスクリプトやアニメーションなどは、Assets/Enemy/Boss の中に新しくフォルダーを作ってその中に置いておく

各Sceneは Assets/Scenes フォルダの中に保存

## ステータス調整

行動頻度、HP、攻撃力、移動速度などはpublic変数にしておき、後から難易度調整をしやすくしてください。

## 敵の攻撃の予備動作

ゲーム的に理不尽にならないように、敵のアニメーションを見てから攻撃を回避できるようにしてください。

特に赤攻撃は、1秒以上は予備動作のアニメーションや予告エフェクトを出す時間を挟むようにしてください。

## 基本の数値・難易度

敵のダメージは基本1、高くても3まで。

ノックバックはx方向に10、y方向に5。

赤攻撃の予備動作は1秒以上。

ボス討伐時間は1~3分、長くても5分

難易度は初プレイの人が5回以内にクリアできるぐらい、難しくても10回ぐらい。

# やらなくても動くがやってほしいこと

## エフェクト

敵がダメージを受けたり倒された時にはダメージエフェクトを出すように

エフェクトを出すには、public GameObject damageEffect;　のようにGameobject 型のpublic変数を作り、変数にエフェクト用のプレファブを入れ、Damage関数にの中で　Instantiate(damageEffect, transform.position, transform.rotation);　を実行すればよい

Enemy/Effect/DamageEffect/Effect_Damage_Enemy.prefab がダメージを受けた時のエフェクト

Enemy/Effect/DamageEffect/Effect_Defeat.prefab　が倒したときのエフェクト

# 入れるとゲーム的に面白くなるかもしれない要素

## スタン

体力を減らしたり、ボスの一部の攻撃中にチャージダッシュでぶつかるとスタンし、一定時間攻撃を加えるチャンスができるようにする。

## 敵の弾をダッシュで反射

ダッシュで敵の弾をはじくと敵に飛んで行ってダメージを与えるようにする。あえて敵の弾にぶつかりにいくのはスリリングでいいかもしれない。

## 未実装の要素(必須)

### 1:ワールドマップ

ボスを討伐すると次のボスの扉が解放される機能が未実装。そもそも宮本がシーンをまたいで情報を保存する方法をよく知らない

### 2:能力解放

ボス討伐による能力の増加。そもそも宮本がシーンをまたいで情報を保存する方法をよく知らない

### 3:データのセーブ

どのボスが討伐されているかの情報の保存。宮本がセーブの実装法を知らない

### 4:ボスのHPゲージ

### 5:赤攻撃の予告エフェクト

# その他の作業効率が上がるかもしれない情報

## コルーチンによる遅延処理

敵の行動などで、何秒後にこの行動をする、といった遅延処理。

## 頻繁に使う内容を書いたスクリプト

CommonScript/Useful に、触れたものにダメージを与えるだけ、まっすぐ飛ぶだけ、出現から一定時間後に消滅などの、敵の弾などで頻繁に使いそうなものをいくつか用意してある。
