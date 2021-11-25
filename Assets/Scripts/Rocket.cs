
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Rocket : MonoBehaviour
{
    // Заморозить позицию в RigidBody Freeze Position по Z
    // Заморозить поворот в RigidBody Freeze Rotation по X, Y
    //Drag - земедление и поставить 0.35
    //Mass - 0.02
    //Back ground - Iditor - mesh render -cast SHADOW-off Отключить тень
    //Edit- QualitySetting - тень Ultra отключить галочку
    //[SerializeField] int Battery = 100;
    [SerializeField] Text energyText;
    [SerializeField] int EnergyTotal = 100;
    [SerializeField] int EnergyApply = 40;
    [SerializeField] float rotSpeed = 100f;
    [SerializeField] float flySpeed = 100f;
    [SerializeField] AudioClip FlySound;
    [SerializeField] AudioClip BoomSound;
    [SerializeField] AudioClip FinishSound;
    [SerializeField] ParticleSystem FlyParticles;
    [SerializeField] ParticleSystem BoomParticles;
    [SerializeField] ParticleSystem FinishParticles;

    bool collisionOff = false;

    Rigidbody rigidBody;
    //AudioSource _audioSource;

    enum State { Playing, Death, NextLevel }
    State _state = State.Playing;

    void Start()
    {
        energyText.text = EnergyTotal.ToString();
        _state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        //_audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {

        if (_state == State.Playing)
        {
            Launch();
            Rotation();
        }
        if (Debug.isDebugBuild)
        {
            DebugKey();
        }

    }
    void DebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            collisionOff = !collisionOff;// переключаем с тру на фолс и наоборот
        }
    }
    void OnCollisionEnter(Collision collision)
    {

        if (_state == State.Death || _state == State.NextLevel || collisionOff) //_state !=State.Death
        {
            return;//если врезался и чтобы не управлять , то выходим из метода
        }

        //Повесить теги на обьекты и назвать Friendly,Finish и тд

        switch (collision.gameObject.tag) //проверка если RigidBody коснулся collision  
        {
            case "Friendly":
                break;
            case "Finish":
                Finish();
                break;
            case "Battery":
                PlusBattety(10,collision.gameObject);
                break;
            default:
                Lose();
                break;

        }
    }
    void PlusBattety(int energyToAdd,GameObject batteryObj)
    {
        batteryObj.GetComponent<SphereCollider>().enabled = false;
        EnergyTotal += energyToAdd;
        energyText.text = EnergyTotal.ToString();
        Destroy(batteryObj);
    }
void Finish()
    {
        _state = State.NextLevel;
        //_audioSource.Stop();
        //_audioSource.PlayOneShot(FinishSound);
        FinishParticles.Play();
        Invoke("LoadNextLevel", 2f);
    }
void Lose ()
    {
        _state = State.Death;
        print("RocketBoom!");
        //_audioSource.Stop();
       // _audioSource.PlayOneShot(BoomSound);
        BoomParticles.Play();
        Invoke("LoadFirstLevel", 2f);
    }
void LoadNextLevel()//finish
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIdex = currentLevelIndex + 1;

        if (nextLevelIdex==SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIdex = 1;
        }
        SceneManager.LoadScene(nextLevelIdex);
    }
void LoadFirstLevel()//lose
    {
        SceneManager.LoadScene(1);
    }
void Launch()
{
        if (Input.GetKey(KeyCode.Space)&&EnergyTotal>0)
        {
            EnergyTotal -= Mathf.RoundToInt(EnergyApply*Time.deltaTime);
            energyText.text =EnergyTotal.ToString();

            rigidBody.AddRelativeForce(Vector3.up* flySpeed*Time.deltaTime);
            FlyParticles.Play();
            //if (_audioSource.isPlaying == false)
            //{
            //    //_audioSource.PlayOneShot(FlySound);
            //    FlyParticles.Play();
            //}


            //else
            //{
            //    //_audioSource.Pause();
            //    FlyParticles.Stop();
            //}
        }
        else
        {
            //_audioSource.Pause();
            FlyParticles.Stop();
        }

    }
    void Rotation()
{
    float rotationSpeed = rotSpeed * Time.deltaTime;
        rigidBody.freezeRotation = true;//чтобы не крутилась при ударе
    if (Input.GetKey(KeyCode.A))
    {
        transform.Rotate(Vector3.forward * rotationSpeed);
    }
    else if (Input.GetKey(KeyCode.D))
    {
        transform.Rotate(-Vector3.forward * rotationSpeed);
    }
        rigidBody.freezeRotation = false;
}
}

