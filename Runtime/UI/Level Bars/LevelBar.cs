using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TransparentGames.Data;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    public Action<int> LevelUp;

    [SerializeField] private Image bar;
    [SerializeField] private LevelableScriptableObject animatedBar;
    [SerializeField] private BaseNode<int> experienceNode;
    [SerializeField] private BaseNode<int> levelNode;

    private Tween _fillTween;

    private void OnEnable()
    {
        OnExperienceChanged();
        experienceNode.AddListener(OnExperienceChanged);
    }

    private void OnDisable()
    {
        _fillTween.Kill();
        experienceNode.RemoveListener(OnExperienceChanged);
    }

    public void AddValue(int value)
    {
        experienceNode.Value += value;
    }

    private void OnExperienceChanged()
    {
        if (_fillTween != null && _fillTween.IsActive())
            _fillTween.Kill();

        AnimateProgress(experienceNode.Value);
    }

    public void AnimateProgress(int currentExperience, float animationTime = 1f)
    {
        int experienceRequired = animatedBar.levels[levelNode.Value];

        if (currentExperience >= experienceRequired)
        {
            levelNode.Value++;

            _fillTween = bar.DOFillAmount(1, animationTime).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                bar.fillAmount = 0;
                AnimateProgress(currentExperience, animationTime / 2);
            });
            return;
        }

        int previousExperience = levelNode.Value == 0 ? 0 : animatedBar.levels[levelNode.Value - 1];

        float targetFilling = (float)(currentExperience - previousExperience) / (experienceRequired - previousExperience);
        bar.DOFillAmount(targetFilling, animationTime).SetEase(Ease.Linear).SetUpdate(true);
    }
}
