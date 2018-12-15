using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{

    public Transform player;
    public float raining_rate;
    public uint rainfall;// how heavy is the rain
    public float rain_start = 0, rain_end = 0;

    // Use this for initialization
    void Start()
    {
        dice_rain();
    }

    // Update is called once per frame
    void Update()
    {
        if (rainfall > 0)
        {
            //turn cloudy
            if (time.interval(GetComponent<time>().world_time, time.add(rain_start, -200), rain_start))
                GetComponent<Light>().intensity -= 0.1f * Time.deltaTime;
            //starts raining
            else if (time.interval(GetComponent<time>().world_time, rain_start, rain_end))
            {
                if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
                for (int i = 0; i < rainfall; i++)
                {
                    GameObject rain = Instantiate(Resources.Load("Rain") as GameObject);
                    rain.transform.parent = this.transform;
                    float posX = Random.Range(player.position.x - 20, player.position.x + 20);
                    float posY = player.transform.position.y + 50;
                    float posZ = Random.Range(player.position.z - 20, player.position.z + 20);
                    rain.transform.position = new Vector3(posX, posY, posZ);
                }
            }
            //turn sunny
            else if (time.interval(GetComponent<time>().world_time, rain_end, time.add(rain_end, 200)))
            {
                if (GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Stop();
                GetComponent<Light>().intensity += 0.1f * Time.deltaTime;
                if (!time.interval(time.add(GetComponent<time>().world_time, GetComponent<time>().delta_time * Time.deltaTime), rain_end, time.add(rain_end, 200)))
                {
                    GetComponent<Light>().intensity += 0.1f;
                    dice_rain();
                }
            }
        }
        //decide raining or not every day
        else if (GetComponent<time>().world_time == 0) dice_rain();
    }

    void dice_rain()
    {
        GetComponent<Light>().intensity = 1;
        if (Random.Range(0, 100) <= 100 * raining_rate)
        {
            rainfall = (uint)Random.Range(10, 40);
            rain_start = time.add(GetComponent<time>().world_time, Random.Range(500, 20000));
            rain_end = time.add(rain_start, Random.Range(500, 3000));
        }
    }

    //0 < last_time < 24000, 10 < heavy < 40
    public void rain(float last_time, uint heavy)
    {
        GetComponent<AudioSource>().Play();
        GetComponent<Light>().intensity = 0;
        rainfall = heavy;
        rain_start = GetComponent<time>().world_time;
        rain_end = time.add(rain_start, last_time);
    }

}