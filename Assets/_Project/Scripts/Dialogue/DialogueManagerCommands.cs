using Mystie.Core;
using Mystie.UI;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Mystie.Dialogue
{
    public class DialogueManagerCommands
    {
        /// <summary>changes background image</summary>
        [YarnCommand("Scene")]
        public static void DoSceneChange(string locationName)
        {
            DialogueManager.Instance.DoSceneChange(locationName);
        }

        [YarnCommand("Contestant")]
        public static void SetActorEpisode(string actorName, string spriteName, string positionX = "", string positionY = "", float alpha = 1f, string colorHex = "")
        {
            //string actorName = LevelManager.Instance.episode.name;
            SpriteLayered dialogueModel = Object.Instantiate<SpriteLayered>(DressupUIManager.Instance.dressupUI.modelUI.model);

            DialogueManager.Instance.AddSprite(actorName, dialogueModel, Vector2.zero, 0f);
            DialogueManager.Instance.SetActor(actorName, spriteName, positionX, positionY, alpha, colorHex);
        }

        /// <summary>
        /// SetActor(actorName,spriteName,positionX,positionY,color) main
        /// function for moving / adjusting characters</summary>
        [YarnCommand("Act")]
        public static void SetActor(string actorName, string spriteName, string positionX = "", string positionY = "", float alpha = 1f, string colorHex = "")
        {
            DialogueManager.Instance.SetActor(actorName, spriteName, positionX, positionY, alpha, colorHex);
        }

        ///<summary> Draw(spriteName,positionX,positionY) generic function
        ///for sprite drawing</summary>
        [YarnCommand("Draw")]
        public static void SetSpriteYarn(string actorName, string positionX = "", string positionY = "")
        {
            DialogueManager.Instance.SetSpriteUnity(actorName, actorName.Split('.'), positionX, positionY);
        }

        ///<summary>Hide(spriteName). "spriteName" can use wildcards, e.g.
        ///HideSprite(Sally*) will hide both SallyIdle and
        ///Sally_Happy</summary>
        [YarnCommand("Hide")]
        public static void HideSprite(string spriteName)
        {
            DialogueManager.Instance.HideSprite(spriteName);
        }

        /// <summary>HideAll doesn't actually use any parameters</summary>
        [YarnCommand("HideAll")]
        public static void HideAllSprites()
        {
            DialogueManager.Instance.HideAllSprites();
        }

        /// <summary>Reset doesn't actually use any parameters</summary>
        [YarnCommand("Reset")]
        public static void ResetScene()
        {
            DialogueManager.Instance.ResetScene();
            HideAllSprites();
            FadeInBG(0);
        }

        // move a sprite usage: <<Move actorOrspriteName, screenPosX=0.5,
        // screenPosY=0.5, moveTime=1.0>> screenPosX and screenPosY are
        // normalized screen coordinates (0.0 - 1.0) moveTime is the time
        // in seconds it will take to reach that position
        [YarnCommand("Move")]
        public static void MoveSprite(string actorOrSpriteName, string screenPosX = "0.5", string screenPosY = "0.5", float moveTime = 0.5f)
        {
            DialogueManager.Instance.MoveSprite(actorOrSpriteName, screenPosX, screenPosY, moveTime);
        }

        /// <summary>flip a sprite, or force the sprite to face a
        /// direction< Move(actorOrSpriteName, xDirection=toggle)</sprite>
        [YarnCommand("Flip")]
        public static void FlipSprite(string actorOrSpriteName, string xDirection = "")
        {
            DialogueManager.Instance.FlipSprite(actorOrSpriteName, xDirection);
        }

        /// <summary>Shake(actorName or spriteName, strength=0.5)</summary>
        [YarnCommand("Shake")]
        public static void ShakeSprite(string actorOrSpriteName, float shakeStrength = 0.5f)
        {
            DialogueManager.Instance.ShakeSprite(actorOrSpriteName, shakeStrength);
        }

        /// <summary>PlayAudio( soundName,volume,"loop" )...
        /// PlayAudio(soundName,1.0) plays soundName once at 100% volume...
        /// if third parameter was word "loop" it would loop "volume" is a
        /// number from 0.0 to 1.0 "loop" is the word "loop" (or "true"),
        /// which tells the sound to loop over and over</summary>
        [YarnCommand("PlayAudio")]
        public static void PlayAudio(string soundName, string loop = "", float volume = 1)
        {
            bool shouldLoop = loop.Contains("loop") || loop.Contains("true");
            AudioManager.Instance.PlayAudio(soundName, shouldLoop, volume);
        }

        /// <summary>stops sound playback based on sound name, whether it's
        /// looping or not</summary>
        [YarnCommand("StopAudio")]
        public static void StopAudio(string soundName)
        {
            AudioManager.Instance.StopAudio(soundName);
        }

        /// <summary>stops all currently playing sounds, doesn't actually
        /// take any parameters</summary>
        [YarnCommand("StopAudioAll")]
        public static void StopAudioAll()
        {
            AudioManager.Instance.StopAudioAll();
        }

        [YarnCommand("FadeIn")]
        public static void FadeInSprite(string actorOrSpriteName, float fadeTime = 0.5f)
        {
            DialogueManager.Instance.FadeSprite(actorOrSpriteName, 0, 1, fadeTime);
        }

        [YarnCommand("FadeOut")]
        public static void FadeOutSprite(string actorOrSpriteName, float fadeTime = 0.5f)
        {
            DialogueManager.Instance.FadeSprite(actorOrSpriteName, 1, 0, fadeTime);
        }

        /// <summary>typical screen fade effect, good for transitions?
        /// usage: Fade( #hexcolor, startAlpha, endAlpha, fadeTime
        /// )</summary>
        [YarnCommand("FadeBG")]
        public static void FadeBG(string fadeColorHex, float startAlpha = 0, float endAlpha = 1, float fadeTime = 1)
        {
            Image fadeBG = DialogueManager.Instance.bgImage;
            DialogueManager.Instance.SetFade(fadeBG, fadeColorHex, startAlpha, endAlpha, fadeTime);
        }

        /// <summary>convenient for an easy fade in, no matter what the
        /// previous fade color or alpha was</summary>
        [YarnCommand("FadeInBG")]
        public static void FadeInBG(float fadeTime = 1)
        {
            Image fadeBG = DialogueManager.Instance.bgImage;
            DialogueManager.Instance.SetFade(fadeBG, fadeTime);
        }

        /// <summary>pan the camera. Usage: CameraOffset(xPos, yPos,
        /// moveTime)</summary>
        /// 0, 0 is center default
        [YarnCommand("CamOffset")]
        public static void SetCameraOffset(string xPos = "", string yPos = "", float moveTime = 0.25f)
        {
            DialogueManager.Instance.SetCameraOffset(xPos, yPos, moveTime = 0.25f);
        }
    }
}
