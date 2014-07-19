using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMORPGCopierClient
{
    public enum GameState : int
    {
        none = 0,
        game_game,
        game_login,
        game_characterselector,
        game_charactercreator,
        game_map,
        login_login,
        login_loggedin,
        character_character,
        character_backtologin,
        character_newcharacter,
        character_newcharacterend,
        character_selectedcharacter,
        map_map,
        map_backtologin,
        map_warp,
        map_warping,
        anim_anim,
        anim_idle,
        anim_walk,
        anim_run,
        anim_attackidle,
        anim_attack1,
        anim_attack2,
        anim_attack3,
        anim_die
    }
}
