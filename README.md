# minigame 2
## Devlog
Draft for student review: edit this section to reflect your own experience and understanding before submitting.

### 1. An issue and how it was fixed

During this part, one issue in the spell script is the unifinished statement. The statement miss parentheses. I actually fixed it by addding `if(timeleft<=0.0f)`. Besides, I delete `Time.Deltatime` from `-timeleft` to make sure that the remaining time will decrease each frame. When the time reaches zero, the spell and its collider are disabled. This finding was found with the help with ChatGPT, I cannot find it by myself since I test it again and again and I can't understand why this situation happen at first.
### 2. My understanding of the color statement

`_spriteRenderer.color = new Color(r, 0.2f, 0.2f);`

I think this line changes the chest's color. The period connects `_spriteRender` to its color property, so the code can change the property. I think `new` creates a new color since I can see r and 0.2f in the parentheses. These there paramount sybolize there-primary colors-red, blue, and green and this cause the chest darker when it is attacked.

## Controls and features
- WASD: move; left mouse button or Space: cast a spell.
- A spell lasts one second; press again after it disappears to cast again.
- Each chest starts with health 4. Hits display h = 3, h = 2, h = 1 with three color stages; the fourth hit removes it.
- Three complete Prop objects occupy Layer 1, Layer 2, and Layer 3. Each has its own health text and matching sprite sorting/collision layers.
- Open `Minigame 2` with Unity 6000.5.6f1, then open `Assets/Scenes/Main.unity`.

## Open-Source Assets
- Pixel art environment & character sprites: https://assetstore.unity.com/packages/2d/environments/pixel-art-top-down-basic-187605
