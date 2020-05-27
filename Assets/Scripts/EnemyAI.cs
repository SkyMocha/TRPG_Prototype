using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EnemyAI
{

    public static void run (EnemyController controller) {
        // Players in attacking range
        Item attackingWeapon = controller.getEntity().getCurrentWeapon();

        List<Player> attackRange = Map.playersInRange(controller.getPos(), attackingWeapon.getRange());

        // Are any players within attacking range?
        if (attackRange.Count == 0)
            return;
        // Attack players within attacking range
        else
            lowestHealth(attackRange).getEntity().damage(attackingWeapon.getDamage(), attackingWeapon.getElemDamage());

    }

    // Finds the player with the lowest health
    public static PlayerController lowestHealth (List<Player> players) {
        Player _p = players[0];
        foreach (Player p in players) {
            if (p.getController().getEntity().getHealth() < _p.getController().getEntity().getHealth())
                _p = p;
        }
        return _p.getController();
    }

}
