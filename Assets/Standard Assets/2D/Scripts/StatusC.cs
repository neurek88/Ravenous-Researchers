using UnityEngine;
using System.Collections;

public class StatusC : MonoBehaviour {

	public string characterName = "";
	public int characterId = 0;
	public int level = 1;
	public int atk = 0;
	public int def = 0;
	public int matk = 0;
	public int mdef = 0;
	public int exp = 0;
	public int maxExp = 100;
	public int maxHealth = 100;
	[HideInInspector]
	public int totalMaxHealth = 100;

	public int health = 100;
	public int maxMana = 100;
	[HideInInspector]
	public int totalMaxMana = 100;

	public int mana = 100;
	public int statusPoint = 0;
	public int skillPoint = 0;
	private bool dead = false;
	public bool immortal = false;
	
	[HideInInspector]
	public GameObject mainModel;
	
	[HideInInspector]
		public int addAtk = 0;
	[HideInInspector]
		public int addDef = 0;
	[HideInInspector]
		public int addMatk = 0;
	[HideInInspector]
		public int addMdef = 0;
	[HideInInspector]
		public int addHP = 0;
	[HideInInspector]
		public int addMP = 0;

	public Transform deathBody;
	
	[HideInInspector]
		public string spawnPointName = ""; //Store the name for Spawn Point When Change Scene
	
	//---------States----------
	[HideInInspector]
		public int buffAtk = 0;
	[HideInInspector]
		public int buffDef = 0;
	[HideInInspector]
		public int buffMatk = 0;
	[HideInInspector]
		public int buffMdef = 0;
	
	[HideInInspector]
		public int weaponAtk = 0;
	[HideInInspector]
		public int weaponMatk = 0;
	
	//Negative Buffs
	[HideInInspector]
		public bool poison = false;
	[HideInInspector]
		public bool silence = false;
	[HideInInspector]
		public bool web = false;
	[HideInInspector]
		public bool stun = false;
	
	[HideInInspector]
		public bool freeze = false; // Use for Freeze Character
	[HideInInspector]
		public bool dodge = false;
	
	//Positive Buffs
	[HideInInspector]
		public bool brave = false; //Can be use for Weaken
	[HideInInspector]
		public bool barrier = false;
	[HideInInspector]
		public bool mbarrier = false;
	[HideInInspector]
		public bool faith = false; //Can be use for Clumsy
	[HideInInspector]
		public bool block = false;
	
	//Effect
	public GameObject poisonEffect;
	public GameObject silenceEffect;
	public GameObject stunEffect;
	public GameObject webbedUpEffect;
	
	public AnimationClip stunAnimation;
	public AnimationClip webbedUpAnimation;
	[System.Serializable]
	public class elem{
		public string elementName = "";
		public int effective = 100;
	}
	public elem[] elementEffective = new elem[5];
	// 0 = Normal , 1 = Fire , 2 = Ice , 3 = Earth , 4 = Wind
	public Resist statusResist;
	[HideInInspector]
	public Resist eqResist;
	[HideInInspector]
	public Resist totalResist;
	[HideInInspector]
	public HiddenStat hiddenStatus;
	[HideInInspector]
	public bool useMecanim = false;
	public string sendMsgWhenDead = "";
	[HideInInspector]
	public bool canControl = true;

	void Start(){
		if(characterName != ""){
			gameObject.name = characterName;
		}
		CalculateStatus();
	}
	
	public string OnDamage(int amount , int element){	
		if (!dead) {
			if(dodge){
				return "Evaded";
			}
			if(immortal){
				return "Invulnerable";
			}
			if(block){
				return "Guard";
			}
			if(hiddenStatus.autoGuard > 0){
				int ran = Random.Range(0 , 100);
				if(ran <= hiddenStatus.autoGuard){
					return "Guard";
				}
			}
			amount -= def;
			amount -= addDef;
			amount -= buffDef;
		
			//Calculate Element Effective
			amount *= elementEffective [element].effective;
			amount /= 100;
		
			if (amount < 1) {
					amount = 1;
			}
		
			health -= amount;
		
			if(health <= 0){
				health = 0;
				enabled = false;
				dead = true;
				Death();
			}
		}
		return amount.ToString();
	}

	public string OnMagicDamage(int amount , int element){
		if(!dead){
			if(dodge){
				return "Evaded";
			}
			if(immortal){
				return "Invulnerable";
			}
			if(block){
				return "Guard";
			}
			if(hiddenStatus.autoGuard > 0){
				int ran = Random.Range(0 , 100);
				if(ran <= hiddenStatus.autoGuard){
					return "Guard";
				}
			}
			amount -= mdef;
			amount -= addMdef;
			amount -= buffMdef;
		
			//Calculate Element Effective
			amount *= elementEffective[element].effective;
			amount /= 100;
		
			if(amount < 1){
				amount = 1;
			}
		
			health -= amount;
		
			if(health <= 0){
				health = 0;
				enabled = false;
				dead = true;
				Death();
			}
		}
		return amount.ToString();
	}
	
	public void Heal(int hp , int mp){
		health += hp;
		if(health >= totalMaxHealth){
			health = totalMaxHealth;
		}

		mana += mp;
		if(mana >= totalMaxMana){
			mana = totalMaxMana;
		}
	}
	
	public void Death(){
		if(sendMsgWhenDead != ""){
			SendMessage(sendMsgWhenDead , SendMessageOptions.DontRequireReceiver);
		}
		if(gameObject.tag == "Player"){
			SaveData();
		}
		Destroy(gameObject);
		if(deathBody){
			Instantiate(deathBody, transform.position , transform.rotation);
		}else{
			print("This Object didn't assign the Death Body");
		}
	}

	
	
	void SaveData(){
		PlayerPrefs.SetInt("PreviousSave", 10);
		PlayerPrefs.SetString("TempName", characterName);
		PlayerPrefs.SetInt("TempID", characterId);
		PlayerPrefs.SetInt("TempPlayerLevel", level);
		PlayerPrefs.SetInt("TempPlayerATK", atk);
		PlayerPrefs.SetInt("TempPlayerDEF", def);
		PlayerPrefs.SetInt("TempPlayerMATK", matk);
		PlayerPrefs.SetInt("TempPlayerMDEF", mdef);
		PlayerPrefs.SetInt("TempPlayerEXP", exp);
		PlayerPrefs.SetInt("TempPlayerMaxEXP", maxExp);
		PlayerPrefs.SetInt("TempPlayerMaxHP", maxHealth);
		PlayerPrefs.SetInt("TempPlayerMaxMP", maxMana);
		PlayerPrefs.SetInt("TempPlayerSTP", statusPoint);
		PlayerPrefs.SetInt("TempPlayerSKP", skillPoint);

		print("Saved");
	}
	
	public void CalculateStatus(){
		addAtk = 0;
		addAtk += atk + buffAtk + weaponAtk;
		//addDef += def;
		addMatk = 0;
		addMatk += matk + buffMatk + weaponMatk;
		//addMdef += mdef;
		totalMaxHealth = maxHealth + addHP;
		totalMaxMana = maxMana + addMP;
		//addMdef += mdef;
		if(health >= totalMaxHealth){
			health = totalMaxHealth;
		}

		if(mana >= totalMaxMana){
			mana = totalMaxMana;
		}
		totalResist.poisonResist = statusResist.poisonResist + eqResist.poisonResist;
		totalResist.silenceResist = statusResist.silenceResist + eqResist.silenceResist;
		totalResist.stunResist = statusResist.stunResist + eqResist.stunResist;
		totalResist.webResist = statusResist.webResist + eqResist.webResist;
	}

	//----------States--------
	public IEnumerator OnPoison(int hurtTime){
		int amount = 0;
		GameObject eff = new GameObject();
		Destroy(eff.gameObject);
		if(!poison){
			int chance= 100;
			chance -= totalResist.poisonResist;
			if(chance > 0){
				int per= Random.Range(0, 100);
				if(per <= chance){
					poison = true;
					amount = maxHealth * 2 / 100; // Hurt 2% of Max HP
				}
			
			}
		//--------------------
			while(poison && hurtTime > 0){
				if(poisonEffect){ //Show Poison Effect
					eff = Instantiate(poisonEffect, transform.position, poisonEffect.transform.rotation) as GameObject;
					eff.transform.parent = transform;
				}
				yield return new WaitForSeconds(0.7f); // Reduce HP  Every 0.7f Seconds
				health -= amount;
			
				if (health <= 1){
					health = 1;
				}
				if(eff){ //Destroy Effect if it still on a map
					Destroy(eff.gameObject);
				}
				hurtTime--;
				if(hurtTime <= 0){
					poison = false;
				}
			}
		}
	}

	
	public IEnumerator OnSilence(float dur){
		GameObject eff = new GameObject();
		Destroy(eff.gameObject);
		if(!silence){
			int chance= 100;
			chance -= totalResist.silenceResist;
			if(chance > 0){
				int per= Random.Range(0, 100);
				if(per <= chance){
						silence = true;
					if(silenceEffect){
						eff = Instantiate(silenceEffect, transform.position, transform.rotation) as GameObject;
						eff.transform.parent = transform;
					}
						yield return new WaitForSeconds(dur);
						if(eff){ //Destroy Effect if it still on a map
							Destroy(eff.gameObject);
						}
						silence = false;
				}
				
			}

		}
	}

	public IEnumerator OnWebbedUp(float dur){
		GameObject eff = new GameObject();
		Destroy(eff.gameObject);
		if(!web){
			int chance= 100;
			chance -= totalResist.webResist;
			if(chance > 0){
				int per= Random.Range(0, 100);
				if(per <= chance){
					web = true;
					freeze = true; // Freeze Character On (Character cannot do anything)
					if(webbedUpEffect){
						eff = Instantiate(webbedUpEffect, transform.position, transform.rotation) as GameObject;
						eff.transform.parent = transform;
					}
					if(webbedUpAnimation){// If you Assign the Animation then play it
						if(useMecanim){
							mainModel.GetComponent<Animator>().SetBool("struggle" , true);
						}else{
							mainModel.GetComponent<Animation>()[webbedUpAnimation.name].layer = 25;
							mainModel.GetComponent<Animation>().Play(webbedUpAnimation.name);
						}
					}
					yield return new WaitForSeconds(dur);
					if(eff){ //Destroy Effect if it still on a map
						Destroy(eff.gameObject);
					}
					if(webbedUpAnimation && !useMecanim){// If you Assign the Animation then stop playing
						mainModel.GetComponent<Animation>().Stop(webbedUpAnimation.name);
					}else{
						mainModel.GetComponent<Animator>().SetBool("struggle" , false);
					}
					freeze = false; // Freeze Character Off
					web = false;
				}
			}
		}
	}

	public IEnumerator OnStun(float dur){
		GameObject eff = new GameObject();
		Destroy(eff.gameObject);
		if(!stun){
			int chance= 100;
			chance -= totalResist.stunResist;
			if(chance > 0){
				int per= Random.Range(0, 100);
				if(per <= chance){
					stun = true;
					freeze = true; // Freeze Character On (Character cannot do anything)
					if(stunEffect){
						eff = Instantiate(stunEffect, transform.position, stunEffect.transform.rotation) as GameObject;
						eff.transform.parent = transform;
					}
					if(stunAnimation){// If you Assign the Animation then play it
						if(useMecanim){
							mainModel.GetComponent<Animator>().SetBool("stun" , true);
						}else{
							mainModel.GetComponent<Animation>()[stunAnimation.name].layer = 25;
							mainModel.GetComponent<Animation>().Play(stunAnimation.name);
						}
					}
					yield return new WaitForSeconds(dur);
					if(eff){ //Destroy Effect if it still on a map
						Destroy(eff.gameObject);
					}
					if(stunAnimation && !useMecanim){// If you Assign the Animation then stop playing
						mainModel.GetComponent<Animation> ().Stop (stunAnimation.name);
					}else{
						mainModel.GetComponent<Animator>().SetBool("stun" , false);
					}
					freeze = false; // Freeze Character Off
					stun = false;
				}
				
			}

		}
		
	}

	public void ApplyAbnormalStat(int statId , float dur){
		if(statId == 0){
			OnPoison(Mathf.FloorToInt(dur));
			StartCoroutine(OnPoison(Mathf.FloorToInt(dur)));
		}
		if(statId == 1){
			//OnSilence(dur);
			StartCoroutine(OnSilence(dur));
		}
		if(statId == 2){
			//OnStun(dur);
			StartCoroutine(OnStun(dur));
		}
		if(statId == 3){
			//OnWebbedUp(dur);
			StartCoroutine(OnWebbedUp(dur));
		}
		
		
	}
	
	public IEnumerator OnBarrier (int amount , float dur){
		//Increase Defense
		if(!barrier){
			barrier = true;
			buffDef = 0;
			buffDef += amount;
			CalculateStatus();
			yield return new WaitForSeconds(dur);
			buffDef = 0;
			barrier = false;
			CalculateStatus();
		}
		
	}

	public IEnumerator OnMagicBarrier(int amount , float dur){
		//Increase Magic Defense
		if(!mbarrier){
			mbarrier = true;
			buffMdef = 0;
			buffMdef += amount;
			CalculateStatus();
			yield return new WaitForSeconds(dur);
			buffMdef = 0;
			mbarrier = false;
			CalculateStatus();
		}
	}

	public IEnumerator OnBrave(int amount , float dur){
		//Increase Attack
		if(!brave){
			brave = true;
			buffAtk = 0;
			buffAtk += amount;
			CalculateStatus();
			yield return new WaitForSeconds(dur);
			buffAtk = 0;
			brave = false;
			CalculateStatus();
		}
	}
	
	public IEnumerator OnFaith (int amount , float dur){
		//Increase Magic Attack
		if(!faith){
			faith = true;
			buffMatk = 0;
			buffMatk += amount;
			CalculateStatus();
			yield return new WaitForSeconds(dur);
			buffMatk = 0;
			faith = false;
			CalculateStatus();
		}
	}

	public void ApplyBuff(int statId , float dur , int amount){
		if(statId == 1){
			//Increase Defense
			StartCoroutine(OnBarrier(amount , dur));
		}
		if(statId == 2){
			//Increase Magic Defense
			StartCoroutine(OnMagicBarrier(amount , dur));
		}
		if(statId == 3){
			//Increase Attack
			StartCoroutine(OnBrave(amount , dur));
		}
		if(statId == 4){
			//Increase Magic Attack
			StartCoroutine(OnFaith(amount , dur));
		}
	}

	public IEnumerator GuardUp(string anim){
		if(block){
			yield break;
		}
		block = true;
		float wait = 0.75f;
		if(!useMecanim && anim != ""){
			//For Legacy Animation
			mainModel.GetComponent<Animation>()[anim].layer = 10;
			mainModel.GetComponent<Animation>().PlayQueued(anim, QueueMode.PlayNow);
			wait = mainModel.GetComponent<Animation>()[anim].length;
		}
		yield return new WaitForSeconds(wait);
		GuardBreak();
	}
	
	public void GuardBreak(){
		block = false;
		/*if(GetComponent<LegacyAnimationSet>()){
			GetComponent<LegacyAnimationSet>().StopGuard();
		}*/
	}
	
	public void FallingDamage(int amount){
		int dmg = maxHealth * amount;
		dmg /= 100;
		print("Falling " + dmg);
		
		health -= dmg;
		if(health <= 0){
			health = 0;
			Death();
		}
	}
}

[System.Serializable]
public class Resist{
	public int poisonResist = 0;
	public int silenceResist = 0;
	public int webResist = 0;
	public int stunResist = 0;
}

[System.Serializable]
public class HiddenStat{
	public bool doubleJump = false;
	public int drainTouch = 0;
	public int autoGuard = 0;
	public int mpReduce = 0;
}
