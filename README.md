# SafeStep-XR: Simulatore Professionale di Emergency Technical Shutdown

<img src="./img/logo.jpg" alt="logo" width="250">

**Versione:** 1.0.0 <br>
**Status:** Production Ready <br>
**Ultimo Aggiornamento:** 17 Aprile 2026 <br>
**Sviluppato durante:** Hackathon Youbiquo

---

## 🏭 Project Overview

### Missione Strategica

SafeStep-XR rappresenta la digitalizzazione avanzata delle procedure di **Emergency Technical Shutdown (ETS)** per ambienti industriali critici, specificamente ottimizzato per il settore **chimico-industriale**. In contesti dove la sicurezza operativa determina direttamente la protezione di impianti chimici ad alta pericolosità, risorse umane e asset aziendali, la preparazione attraverso simulazioni immersive diventa una necessità operativa irrinunciabile.

### Contesto Industria Chimica

Il settore chimico rappresenta uno degli ambiti industriali con il rischio operativo più elevato. Gli emergency shutdown ricoprono un ruolo critico nel prevenire:
- **Rilasci di sostanze tossiche** in atmosfera o corpi idrici
- **Reazioni esotermiche incontrollate** che possono provocare esplosioni
- **Perdita di contenimento** di composti pericolosi
- **Cascata di failure** tra unità di processo interconnesse

SafeStep-XR fornisce ai tecnici industriali chimici uno strumento di simulazione immersiva per consolidare procedure critiche senza rischio.

### Problematica Affrontata

Gli operatori industriali affrontano scenari di emergenza altamente complessi che richiedono:
- **Precisione procedurale**: Esecuzione rigida di Standard Operating Procedures (SOP)
- **Decisionalità sotto pressione**: Gestione di variabili fisiche in tempo reale
- **Conformità normativa**: Tracciamento dei tempi critici (SLA)
- **Feedback diagnostico**: Analisi personalizzata delle performance

### Soluzione Proposta

SafeStep-XR offre una piattaforma XR immersiva per il training di procedure di emergenza nel settore chimico:

| Aspetto | Beneficio |Applicazione Industria Chimica|
|--------|----------|------------------------------|
| **Immersione XR** | Esperienza spaziale dei sistemi fisici reali su Leonardo XR |Replica fedele di unità di processo |
| **FSM Rigorosa** | Garantisce sequenzialità procedurale senza deviazioni |Impedisce deviazioni critiche da SOP chimiche |
| **Fisica Misurabile** | Parametri reali (pressione, temperatura) influenzano l'outcome |Simulazione termodinamica di reazioni chimiche incontrollate |

---

# 🏛️ Flusso VR: Main Hall e Selezione Scenari

Ambiente iniziale (Lobby/Hub) in cui l'operatore VR viene accolto prima di iniziare l'addestramento. L'interfaccia di selezione è integrata direttamente nell'ambiente virtuale (UI) per mantenere alta l'immersività.

## 🔄 User Journey (Navigazione Menu)
1. **Spawn (Ingresso):** L'operatore si materializza all'interno della Main Hall.
2. **Interfaccia TV:** Davanti all'utente è presente uno schermo televisivo interattivo che funge da menu principale del simulatore.
3. **Selezione Modulo:** Tramite i controller VR (puntatore laser), l'operatore seleziona uno dei due scenari di emergenza disponibili.
4. **Transizione:** Al click, il sistema avvia il caricamento e teletrasporta istantaneamente l'utente nella scena operativa selezionata.

## 🎯 Destinazioni Possibili
* ➡️ **Scenario 1 - Guasto a Cascata:** Avvia il modulo complesso in tre fasi (Isolamento SCADA, Raffreddamento Reattore ed Evacuazione guidata).
* ➡️ **Scenario 2 - Emergenza Reattore:** Avvia il modulo incentrato sul guasto pneumatico singolo, uso dei DPI e chiusura manuale della valvola.

# 🏭 SCENARIO 1: Emergenza Reattore

Scenario di addestramento VR per la gestione di un guasto pneumatico industriale, basato su limiti di tempo (SLA) e rispetto dei protocolli di sicurezza.

## 🔄 User Journey
1. **Avvio Emergenza:** La pressione del reattore inizia a salire e parte il timer SLA (2 minuti).
2. **Allarme (4.5 bar):** Scattano in automatico sirena audio e luci rosse lampeggianti.
3. **Messa in Sicurezza:** L'operatore si sposta alla postazione e indossa fisicamente i DPI (casco e guanti).
4. **Intervento:** L'operatore raggiunge il reattore e ruota la valvola a 360° per sfiatare la pressione.

## 🎯 Esiti Possibili
* ✅ **Successo:** La valvola viene chiusa in tempo **con i DPI indossati**. Pressione ripristinata e allarmi spenti.
* ❌ **Infortunio:** L'operatore ruota la valvola **senza DPI**. L'azione viene respinta e l'emergenza continua.
* 💥 **Game Over (Timeout):** Il tempo scade (pressione a 8.5 bar). Il reattore cede e la simulazione viene interrotta.

---

# 🏭 SCENARIO 2: Guasto a Cascata ed Evacuazione

Scenario di addestramento multi-fase. L'operatore è supportato da un HUD dinamico (istruzioni e timer) e da un sistema di Wayfinding visivo a terra per risolvere un guasto a catena e mettersi in salvo.

## 🔄 User Journey (Fasi dell'Emergenza)
1. **Sovraccarico (Fase 1):** Scatta l'allarme. L'HUD mostra il timer SLA e la temperatura del reattore. Si accende il percorso luminoso, con pedane di teletrasporto. L'operatore deve isolare il robot tramite SCADA.
2. **Rischio Fusione (Fase 2):** Robot isolato. L'HUD aggiorna le istruzioni e si accende il percorso luminoso 🟠 verso il Reattore R-4 (Tank). L'operatore deve avviare il raffreddamento.
3. **Evacuazione (Fase 3):** Reattore in sicurezza. L'HUD ordina l'evacuazione immediata. Si accende il percorso luminoso 🟢 verso l'uscita (Exit).
4. **Fuga:** L'operatore entra nell'area di uscita. La porta di emergenza si apre automaticamente (animazione fluida) garantendo la via di fuga.

## 🎯 Esiti Possibili
* ✅ **Vittoria:** L'operatore completa l'intera catena (Isolamento ➡️ Raffreddamento ➡️ Uscita) prima dello scadere del tempo. L'HUD mostra il messaggio di successo verde.

* ❌ **Game Over (SLA Failed):** Il timer scade prima della risoluzione. I percorsi luminosi a terra si spengono e l'HUD mostra a tutto schermo la motivazione del fallimento.

---

## 🎯 Core Pillars

### 1. SOP Compliance: Rigore Procedurale Gestito via FSM

La conformità procedurale è garantita attraverso una **Finite State Machine (FSM) rigida** che modella la sequenza deterministica di azioni di emergenza nel contesto chimico-industriale.

Ogni stato rappresenta una fase critica della procedura di shutdown (ad es. "Isolamento circuiti", "Spegnimento reattori", "Dump di emergenza", "Neutralizzazione prodotti"). L'FSM **impedisce il saltare di step** e consente solo transizioni valide secondo la procedura.

**Vantaggi implementativi:**
- ✅ Impedisce l'esecuzione non ordinata di procedure critiche (conformità SEVESO)
- ✅ Logging automatico di ogni transizione di stato con timestamp
- ✅ Recovery deterministico da stati di emergenza
- ✅ Conformità a standard industriali (IEC 61511, IEC 61513, ATEX)
- ✅ Tracciabilità completa per audit normativo

---

### 2. SLA Tracking: Monitoraggio dei Tempi Critici

Ogni operazione critica è sottoposta a **vincoli temporali (SLA - Service Level Agreement)**, calibrati sui tempi fisici reali del processo chimico:

| Procedura | SLA Massimo | Criticità | Impatto Chimico |
|-----------|------------|-----------|----------|
| **Isolamento circuiti** | 30 secondi | Critica | Prevenzione backflow tossico |
| **Estinzione reattori** | 60 secondi | Critica | Arresto reazione esotermica |
| **Sequenza valvole dump** | 90 secondi | Alta | Scarico controllato in sicurezza |
| **Neutralizzazione prodotti** | 180 secondi | Alta | Controllo chimico della soluzione |
| **Verifica stato sicuro** | 300 secondi | Media | Conferma pressione/temp normale |

**Implementazione:**
- Contatori in tempo reale nel HUD olografico
- Flag di violazione SLA registrati in telemetria
- Alert acustici e visivo

---

### 3. Measurable Physics: Parametri Reali che Influenzano l'Outcome

La simulazione utilizza **parametri fisici e chimici calibrati** sulla realtà del processo, che determinano direttamente l'esito della procedura di emergenza:

**Parametri Chimico-Industriali Monitorati:**
- **Pressione Sistema** (bar) — Controllo sovrapressione
- **Temperatura Circuito** (°C) — Monitoraggio reazioni esotermiche

---

## 🏗️ Technical Architecture

### Stack Tecnologico

| Componente | Versione | Ruolo |
|-----------|---------|-------|
| **Unity** | 2022.3 LTS | Runtime engine |
| **XR Interaction Toolkit** | 2.5.x | Sistema interazione XR |
| **TextMeshPro** | 3.2.x | Rendering UI avanzato |
| **C# / async-await** | .NET Standard 2.1 | Logica asincrona |

---

### Pattern Architetturale

#### **A. ScriptableObjects per Configurazione**

Ogni scenario di emergenza chimica, parametro termodinamico e SOP è definito tramite **asset di configurazione** (ScriptableObjects):

Questa architettura consente di:
- 🎚️ **Scalabilità**: Aggiungere nuovi scenari chimici (reattori batch, continuous flow) senza recompilazione
- ⚡ **Hot-swapping**: Modificare parametri di processo (pressione setpoint, temp limite) da editor senza riavvio
- 📊 **Serializzazione**: Esportazione dati telemetrici in formato strutturato per audit SEVESO
- 🔄 **Versionamento**: Tracciamento delle iterazioni procedurali con changelog

---

#### **B. Finite State Machine Rigida**

L'FSM modella la sequenza deterministica di operazioni chimiche di emergenza con una **matrice di transizione rigida** che consente solo i passaggi previsti dalla SOP.

Ogni stato rappresenta una fase critica:
- **Idle** → **Isolamento** (chiusura valvole di isolamento)
- **Isolamento** → **Estinzione** (spegnimento reattori)
- **Estinzione** → **Dump** (apertura valvole scarico emergency)
- **Dump** → **Verifica** (controllo parametri a livelli sicuri)
- **Verifica** → **SafeState** (procedura completata)

**Benefici:**
- 🛡️ Impedisce anomalie procedurali (ad es. dump senza isolamento)
- 📝 Audit trail completo con timestamp per conformità normativa
- 🔍 Identificazione rapida della causa radice di errori

---

## 📱 Hardware & Interazioni

### Sistema di Interazione XR

#### **XR Grab Interactables**

Ogni elemento fisico interagibile nel contesto chimico (valvola, interruttore di isolamento, pompa di emergenza, leva di dump) è reso **manipolabile** nel controllo dell'operatore.

Questa interazione consente:
- **Rotazione precisa** di valvole (0° = chiuso, 90° = aperto)
- **Feedback visivo** di range operativi validi (zona verde=sicuro, gialla=attenzione, rossa=vietato)
- **Snap automatico** a posizioni significative per la procedura chimico (posizioni discrete)
- **Timestamp di interazione** registrato per audit trail

---

### HUD Olografico in World Space

Il display informativo è ancorato allo spazio mondiale, posizionato in prossimità dell'operatore come pannello di controllo virtuale:

**Elementi HUD Chimico-Industriali:**
- 📊 **Manometro pressione** — 0-100+ bar, con zone di rischio colorate
- 🌡️ **Termometro temperatura** — -20 a +250°C, indicatore reazione esotermica
- ⏱️ **SLA Timer countdown** — Tempo rimanente per fase critica
- 🔴 **Stato valvole** — Open/Closed/Transitioning su piccolo schematico
- 🚨 **Alert bar** — Notifiche di pericolo chimico imminente

---

## 🚀 Perché Investire in SafeStep-XR? (Executive Pitch)

**Il Problema:** Nell'industria chimica, gli errori sotto stress causano disastri milionari. Il training tradizionale (su carta o video) non crea la "memoria muscolare" necessaria per reagire alle vere emergenze industriali.

**La Soluzione:** **SafeStep-XR** è una piattaforma VR enterprise che permette agli operatori di simulare *Emergency Shutdown* critici in un ambiente immersivo, sicuro e basato su parametri fisici reali.

### 💎 I 3 Motivi per Investire (Value Proposition)

* 💰 **ROI Diretto e Risk Mitigation:** La nostra architettura software costringe l'operatore a rispettare rigidamente i protocolli (SOP). Tracciamo ogni tempo di reazione e ogni errore, fornendo dati oggettivi che le aziende possono usare per la **compliance normativa (SEVESO/ATEX)** e per **abbattere i premi assicurativi**.
* 📈 **Architettura Altamente Scalabile:** Non abbiamo costruito un "singolo videogioco", ma un motore modulare. Aggiungere nuovi scenari, nuovi reattori o espanderci in nuovi settori (Oil & Gas, Nucleare) richiede costi e tempi di sviluppo minimi. È un modello SaaS pronto per scalare B2B.
> **In sintesi:** SafeStep-XR non è un semplice simulatore. È il nuovo standard tecnologico per la sicurezza, la certificazione e la prevenzione disastri nell'industria pesante. 

---

## 🚦 Future Roadmap

### **Fase 2.0: Multiplayer Simulation (Q3 2026)**

**Obiettivo:** Simulazioni collaborative in tempo reale con team di operatori chimici

- Networking layer per sincronizzazione stato FSM tra più utenti
- Ruoli assegnati: Operator A (Isolamento/Pressione), Operator B (Sequenza Valvole), Operator C (Monitoraggio)
- HUD condiviso con feedback collettivo su compliance procedurale
- Audit trail distribuito con timestamp precisi per conformità normativa

**Target:** Validazione in ambiente industriale reale Q4 2026

---

### **Fase 2.1: Digital Twin Integration (Q4 2026)**

**Obiettivo:** Sincronizzazione bi-direzionale tra simulatore e impianti chimici reali

- Connessione MQTT a sensori di impianto (pressione, temperatura, portata)
- Replicazione fedele degli stati fisici attuali in Virtual Environment
- Predictive analytics: anomaly detection, forecasting di failure, optimization di SOP
- Historian telemetrico (InfluxDB) per trend analysis storico
- Dashboard analytics (Grafana) accessibile da control room

---

### **Fase 3.0: Enterprise Deployment (2027)**

**Obiettivo:** Piattaforma enterprise scalabile per catena globale fornitori chimici

- Infrastruttura cloud (Kubernetes) per hosting centralizzato
- Database multi-tenant (PostgreSQL + AWS Aurora)
- Modello licensing seat-based con tiering
- RBAC completo (Role-Based Access Control)
- Audit trail ISO 27001 e conformità GDPR
- Programma di certificazione per operatori chimici

---

## 👥 Il Team (SafeStep-XR)


| Team Member | Ruolo nel Progetto | Contributo Principale |
| :--- | :--- | :--- |
| 🚀 **Matteo** | *Team Leader* | Gestione architettura di base, setup del repository GitHub, versioning e integrazione delle build VR, creazione asset MainHall e ProductionLine, generazione di modelli 3D e ricerca asset, testing. |
| 🥽 **Francesco** | *XV/VR Creator* | Sviluppo logica di simulazione (FSM, SLA Timer) per scenario 2 (reattore) e creazione assset, programmazione `ReactorManager`, gestione trigger eventi ed exit routing. |
| ⚙️ **Emanuel** | *Dev* | Sviluppo e implemetazione della logica del Menù iniziale, implementazione script necessari, integrazione generale, bug fixing. |
| 🎨 **Simone** | *UX/UI Designer* | Progettazione degli spazi 3D e della UI, implementazione logica di teletrasporto, bug fixing, general development, testing. |
| 🖥️ **Luigi** | *XV/VR Creator* | Sviluppo logica di simulaziome completa (FSM, SLA Timer) per scenario 1 (production line), implementazione script `ScenarioManager` e integrazione con il resto della logica implementata, gestione trigger eventi. |

---

---

## 🛠️ Note di Sviluppo e Limitazioni (Hackathon)

> [!IMPORTANT]  
> **Nota sui limiti di tempo:** A causa della durata limitata dell'Hackathon, alcune funzionalità di "Quality of Life" sono state posticipate per dare priorità alla solidità della logica di simulazione e alla stabilità del sistema FSM.

### 🔄 Sviluppi Futuri Prioritari
* **Sistema di Navigazione Scene Interattivo:** Implementazione di un pulsante fisico (UI 3D) a fine simulazione (sia in caso di vittoria che di fallimento) per permettere all'operatore di tornare istantaneamente alla *Main Hall* e selezionare un nuovo scenario senza dover riavviare l'applicazione.
* * **Migliorie grafiche:** Migliorare graficamente la scena (ES. frecce nello scenario 2).
* **Sistema di Reset Dinamico:** Gestione del reset totale dei parametri (pressione, timer, posizione oggetti) per consentire sessioni di training ripetute nello stesso scenario senza ricaricare la scena di Unity.
* **Aggiunta di AI_Evaluator:** Integrazione di un'agentAI esterno richiamato tramite API che da consigli e delucidazioni su noramtive tecniche all'utente durante la simualazione oltre che una valutazione sull'operatore dell'utente una volta finita la simulazione

---


**Versione Documento:** 1.0.0 | **Data:** 17 Aprile 2026 | **Stato:** Production Ready

---
