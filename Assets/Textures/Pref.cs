using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Game;

namespace Unity.LEGO.UI
{
    public class Pref : MonoBehaviour
    {
        public int levelnum;

        bool m_GameOver;
        bool m_Won;
        int level;


         void Start()
        {
            EventManager.AddListener<GameOverEvent>(OnGameOver);    
        }

         void Update()
        {
            if (m_GameOver)
            {
                // Save level.
                if (m_Won){

                    if (!PlayerPrefs.HasKey ("Level"))
                    {
                        PlayerPrefs.SetInt("Level", levelnum);
                    } 
                    else 
                    {
                        level = PlayerPrefs.GetInt("Level");

                        if (level <= levelnum)
                         {
                            PlayerPrefs.SetInt("Level", levelnum);
                         }
                    
                    }
       

                    
                }
                  
            }
        }

        void OnGameOver(GameOverEvent evt)
        {
            if (!m_GameOver)
            {
                m_GameOver = true;
                m_Won = evt.Win;
            }
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        }

    }
}
