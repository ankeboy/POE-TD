# POE-TD
 
## A simple tower defense games with items.

The aim of the game is to build towers, equip items on them and defeat the waves.

Towers can be upgraded to increase various stats and item slots.

Towers have item slots for equiping items

Items are gained at the start of each round (choose one item from the options). (WIP: enemies have a very low chance to drop items).

### Towers:

* standard turret  -  a normal turret that shoots projectiles
* missile turret   -  a long range turret that shoots missiles that track enemies. Upon impact it explodes and deals damage to surrounding enemies
* laser turret     -  a short range turret that aims laser at the enemy. The damage is instance and keeps damaging over time as long as the laser is active. It also slows enemies.
* ice tower        -  a short range tower that damages all enemies in the area (WIP: slows enemies)
* artilery         -  a long range tower that throws slow moving projectile and deals AOE damage the projectile leaves burning ground that inflicts damage over time.)
* sniper           -  a very long range tower with very high single target projectile damage but has a very slow fire rate. It has a basic chance to deal critical hits for extra damage and stun.
* Bonus House      -  a "tower" that does not do anything special but gives you an additional item at the end of the round.

### Items (skills):

* Increased Fire Rate - Adds a fire rate multiplier (1.5x)
* Increased Damage    - Adds a damage modifier (1.5x)
* Double attack       - deals an additional attack *doesn't work with laser tower* (not stackable)
* Increased Range     - Adds a range multiplier (1.5x)
* Critical Chance     - Adds a chance to deal a critical hit. critical hits have increased damage and stun the target
* Increased Projectile Speed - Adds a projectile speed modifier
* burning             - Adds a Damage of Time (DoT) to hit enemies based on the damage of the hit.
* weaken              - Adds a debuff to the enemy that increase the damage taken and reduces the regeneration rate.
* (WIP: Generosity    - Enemies killed by the tower gives more Money)
* (cancellled: Increased Effect - Increases the effect of status ailments, e.g. Slow)
* (cancellled: Pierce        - Projectiles are able to pierce one additional enemy)


### Other features:

* Randomized item generation so that player have to plan accordingly.
* Enemies can remove more than 1 live.
* Enemy variation: enemies can have "accessories" to give them unique characteristics.
* Leaked enemies give double their bounty back.
* levels are unlocked in batches based on difficulty.
* Bullets have a limited lifespan.
* Player has levels and gain experience whenever finishing a game based on how far the player progressed.
* Passive Skill tree system. Players gain skill points to level up passive skills to make games easier.
* Library: a collection of detailed information on all enemies, towers and items (skills).
* Saves the highest wave reached for each level.
* Tooltips

## Important restrictions

1. Tower selling refunds 50% of the base cost (WIP: increase refund gold for upgraded towers)
2. Selling towers with items on them won't return the items it has equiped.
3. Once an item has been equiped on the tower, it cannot be removed or retrieved. So choose wisely.
4. The inventory has 9 slots. Picking up any more items wont add it to the inventory and the item will be lost.

## Future additions

* Add highscore system (Damage calculation on dummy target after the last wave)
* Add upgrade choices and longer upgrade path

## Assets and Copyright

All assets used in this game are placeholders and used under the Creative Commons license.
* Some turrets are used from the [Brackeys TD tutorials](https://www.youtube.com/watch?v=beuoNuK2tbk&list=PLPV2KyIb3jR4u5jX8za5iU1cqnQPmbzG0)
* Item icons are credited to [pauliuw](https://opengameart.org/content/modified-and-cliped-magic-skill-item-icons) from OpenGameArt.
