﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelGenerator : MonoBehaviour
{
    public float level_size = 60;
    public float slide_speed = 1;

    private Vector3 from_target;
    private Vector3 to_target;
    private float t = 0;

    private Transform slide_transform;
    private Transform current_level;
    private Transform new_level;
    private bool level_generating = false;

    [Header("Segment Prefabs")]
    [SerializeField] List<GameObject> top_left_segments = new List<GameObject>();
    [SerializeField] List<GameObject> top_right_segments = new List<GameObject>();
    [SerializeField] List<GameObject> bottom_left_segments = new List<GameObject>();
    [SerializeField] List<GameObject> bottom_right_segments = new List<GameObject>();
    [Space]

    [SerializeField] UnityEvent on_generation_end;
    [SerializeField] UnityEvent on_generation_begin;


    void Start()
    {
        InitLevel();
    }


    void InitLevel()
    {
        slide_transform = new GameObject().transform;
        slide_transform.position = Vector3.zero;
        slide_transform.name = "Level Slider";
        GenerateLevel();
    }


    public void GenerateLevel()
    {
        if (level_generating)
            return;

        Time.timeScale = 0;
        level_generating = true;

        new_level = new GameObject().transform;//new gameobject is the same as instantiating
        new_level.name = "Generated Level";
        new_level.parent = slide_transform;
        new_level.transform.position = new Vector3(level_size, 0, 0);

        if (current_level == null)
            new_level.transform.position = Vector3.zero;

        from_target = slide_transform.position;
        to_target = slide_transform.position + new Vector3(-level_size, 0, 0);

        CreateMap();
        on_generation_begin.Invoke();
    }


    private void CreateMap()
    {
        Vector3 top_left = new Vector3(-(level_size) * 0.25f, 0, -(level_size) * 0.25f);//corner coordinates
        Vector3 top_right = new Vector3(-(level_size) * 0.25f, 0, (level_size) * 0.25f);
        Vector3 bottom_left = new Vector3((level_size) * 0.25f, 0, -(level_size) * 0.25f);
        Vector3 bottom_right = new Vector3((level_size) * 0.25f, 0, (level_size) * 0.25f);

        GenerateSegment(top_left_segments, top_left);
        GenerateSegment(top_right_segments, top_right);
        GenerateSegment(bottom_left_segments, bottom_left);
        GenerateSegment(bottom_right_segments, bottom_right);
    }


    private void GenerateSegment(List<GameObject> _map_segments, Vector3 _position)
    {
        _map_segments.Shuffle();//randomise
        var new_segment = Instantiate(_map_segments.First());
        new_segment.transform.position = new_level.position + _position;
        new_segment.transform.parent = new_level;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.G))//debug
            GenerateLevel();

        if (current_level == null)
            GenerateLevel();

        if (!level_generating)
            return;

        t += Time.unscaledDeltaTime * slide_speed;//unscaled so it doesn't pause
        if (t >= 1)
        {
            EndGeneration();//end generation when lerp is finished
            return;
        }

        slide_transform.position = Vector3.Lerp(from_target, to_target, t);
    }


    private void EndGeneration()
    {
        Time.timeScale = 1;
        t = 0;
        level_generating = false;

        if (current_level)
        {
            DestroyImmediate(current_level.gameObject);//destroy old level
        }

        current_level = new_level;
        current_level.parent = slide_transform;
        new_level = null;
        on_generation_end.Invoke();
    }


    private void OnLevelWasLoaded(int _level)
    {
        InitLevel();
    }
}
