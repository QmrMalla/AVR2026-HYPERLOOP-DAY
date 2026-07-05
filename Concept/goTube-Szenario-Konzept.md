# Szenario: „Öffnen des goTube-Gates" — Konzept & Umsetzung

*AVR2026-HYPERLOOP-DAY · Soloprojekt · Meta Quest 2/3 · Unity 2022.3 LTS*

---

## 1. Kontext & Motivation

Die **goTube** ist eine 27 m lange Hyperloop-Stahlröhre (Ø 1,6 m) des **Institute of Hyperloop Technology** an der **Hochschule Emden/Leer** — die derzeit längste Hyperloop-Testanlage Deutschlands. Sie ist Teil des Interreg-Projekts **HyperBRIDGE** (Deutschland–Niederlande), das u. a. **immersive VR-Trainings für Ingenieur:innen** zum Ziel hat, um sie auf reale Aufgaben am Hyperloop vorzubereiten.

Dieses VR-Szenario greift genau dieses Ziel auf: Es bildet die **reale Türöffnungsprozedur** des goTube-Gates nach und vermittelt dabei das **Vakuum-/Druckausgleich-Prinzip**. Damit ordnet sich das Soloprojekt direkt in die Ziele der realen Forschungsinfrastruktur ein.

## 2. Physikalischer Kern (Lerninhalt)

- In der Röhre herrscht ein **Soft-Vakuum (~100 Pa ≈ 0,1 % des Atmosphärendrucks)**. Die Röhre wirkt als **Druckbehälter**.
- Wegen der **Druckdifferenz** ist das Gate gesperrt und lässt sich nicht direkt öffnen.
- Das **Frontlock darf nicht einfach zur Entlüftung geöffnet werden** — die Belüftung erfolgt ausschließlich über **spezielle Ventile und ein Steuerverfahren** (= Druckausgleich). Dies ist der zentrale Lernmoment.

## 2a. Lernfrage (interaktiver Quiz-Hotspot)

**Frage:** „Wenn die Röhre evakuiert ist — darf ich das Frontlock einfach öffnen, um die Röhre zu belüften?"

**Antwort:** **Nein.** Der Atmosphärendruck übt auf das Frontlock eine Kraft von **≈ 200 kN** aus (entspricht dem Gewicht von ~20 t). Daher muss zur normalen Belüftung ein **Steuerverfahren mit speziellen Ventilen** verwendet werden.

> *Herleitung:* F = Δp · A ≈ 101 325 Pa · π·(0,8 m)² ≈ 101 325 Pa · 2,01 m² ≈ **204 kN** (Röhren-Ø 1,6 m, Druckdifferenz ≈ 1 bar). Diese enorme Kraft ist der physikalische Grund, warum vor dem Öffnen ein Druckausgleich nötig ist.

> In VR als **Frage-Hotspot** umsetzen (wie in der realen 360°-Tour): kurze Ja/Nein- oder Multiple-Choice-Frage, die den Lernkern aus Abschnitt 2 direkt abprüft. Geringer Aufwand, hoher didaktischer Wert.

## 3. Ablauf der Türöffnung (Schritt für Schritt)

| Schritt | Inhalt |
|--------|--------|
| **1 — Visuelle Inspektion** | Kabel, Schalter, Sensoren sowie das Seilzug-/Umlenkrollensystem über der Tür prüfen. |
| **2 — Leitstand** | Röhrendruck am Manometer ablesen, bei Bedarf entlüften; Hauptschalter ein → Türbetrieb aktivieren; Innen-/Außenbeleuchtung einschalten. |
| **3 — Flügelschrauben** | Lösen und zurückschwenken, bis der **induktive Näherungssensor** (z. B. Telemecanique XS7C4A1DPG13, Sn = 15 mm) die korrekte Stellung per **LED** bestätigt. |
| **4 — Sicherheitsprüfungen** | Freigängigkeit der Tür in der Führung (ggf. lösen); **Sicherungsbolzen** seitlich in korrekter Position (LED); **Fahrweg/Bewegungsbereich frei**. |
| **5 — Position am Bedienpanel** | Am Gate-Control-Panel stehen; Bewegungsbereich (gelbe Bodenmarkierung) visuell freihalten. |
| **6 — Totmannschalter & Öffnen** | Sicherheitsschalter ein, Steuerung ein; **zwei Taster gleichzeitig** (Totmann-Prinzip → Hände außerhalb des Bewegungsbereichs); Gate **gleitet vertikal nach oben** in seine obere Endstellung; während des gesamten Vorgangs (bei gedrückten Tastern) durchgehend auf Störungen/Gefahren achten und korrektes Stoppen sicherstellen. |
| **7 — Sichern in Offenstellung** | Sobald das Gate steht und sich in seiner **oberen Endstellung** befindet: **Sicherungsbolzen unter das Gate** setzen, damit es sich nicht mehr bewegen kann (sichere Position). |

> **Bei Störung / Notfall:** Bei jeder Unregelmäßigkeit oder Fehlfunktion sofort den **Sicherheitsschalter ausschalten** bzw. den **NOT-AUS** drücken. Anschließend die Störung untersuchen — dabei stets die **eigene Sicherheit** wahren und **außerhalb des Gefahren-/Bewegungsbereichs** bleiben. Bei Bedarf den Bereich durch eine **Absperrung** sichern, um eine sichere Umgebung herzustellen.

## 4. VR-Umsetzung (Mapping)

> **Leitprinzip (gemäß Dozenten-Feedback):** Das Konzept **beschreibt** den vollständigen Ablauf (alle Schritte wie in der 360°-Tour) — das zeigt das Verständnis der realen Prozedur. Die **VR-Umsetzung** beschränkt sich bewusst auf den **Lernkern** und nutzt vorhandene goTube-Assets. Es wird **nicht jeder Schritt** als eigenes interaktives Asset umgesetzt.

**Zuordnung Schritt → Umsetzung:**

| Schritt | Umsetzung | Notiz |
|--------|-----------|-------|
| 1 Inspektion | Ambient | Blick/Highlights, optional Audio |
| 2 Leitstand (Druck/Belüftung) | **Interaktiv** | Manometer + Druckausgleich = Kern-Physik |
| 3 Flügelschrauben | **Interaktiv** | Greifen & drehen → Winkel = „Sensor" → LED |
| 4 Sicherheitsprüfungen | Ambient | Safety-Checkliste (✓) |
| 5 Position/Bewegungsbereich | Ambient | gelbe Bodenzone visuell freihalten |
| 6 Totmann & Öffnen | **Interaktiv** | zwei Taster gleichzeitig → Tor gleitet nach oben |
| 7 Sichern (Bolzen) | Ambient | Bestätigung/LED |

**Bedienpanel (3D statt Canvas-UI):** Manometer, „Gate operation start", **NOT-AUS**, Beleuchtungsschalter, Status-LEDs — als greifbarer Schaltschrank (Poke/Grab, Meta XR SDK). Erhöht Realismus und akademische Nähe zur echten Anlage.

**Tür-Animation (vertikales Schiebetor):** Das Gate **schwingt nicht** — es **gleitet vertikal nach oben** (angetrieben über Seilzug/Winde oben am Tor) in seine **obere Endstellung** und wird dort mit **Sicherungsbolzen von unten** gegen Zurückgleiten gesichert. In Unity wird daher die **Position animiert (Translation nach oben)** — **keine Rotation, kein Hinge**. Das Gate (`TubeGate1`) gleitet entlang seiner lokalen Y-Achse; bei geneigter Führung das Tor unter ein passend ausgerichtetes Empty hängen.

```csharp
// In Update(): vertikales Gleiten nach oben (Schiebetor).
// closedPos in Start() merken: closedPos = doorLeaf.localPosition;
if (doorIsOpening && doorLeaf != null)
{
    Vector3 target = closedPos + Vector3.up * openDistance;   // openDistance am Asset pruefen
    doorLeaf.localPosition = Vector3.MoveTowards(doorLeaf.localPosition, target, doorSpeed * Time.deltaTime);
    if (Vector3.Distance(doorLeaf.localPosition, target) < 0.01f) { doorIsOpening = false; doorIsOpen = true; }
}
```

## 5. Umsetzungsstand (realisierte VR-Anwendung)

Die VR-Anwendung wurde vollständig in Unity umgesetzt und läuft als eigenständige App (Build/APK) auf der **Meta Quest 2**. Realisierte Funktionen:

- **Druckausgleich (Kern-Physik):** Über einen Poke-Taster am Bedienpanel wird der Druckausgleich gestartet; der Druckwert steigt von 0 auf 1 bar. Ein **Manometer mit drehendem Zeiger** zeigt den Verlauf live an. Vor abgeschlossenem Druckausgleich bleibt das Gate gesperrt (mit Hinweis auf die ≈ 200 kN Druckkraft).
- **Vertikales Schiebetor:** Nach dem Druckausgleich gleitet das Gate (`TubeGate1`) über eine Translation nach oben in seine obere Endstellung – keine Rotation.
- **Totmann-Prinzip:** Zwei Poke-Taster auf der ElectricBox müssen gehalten werden, damit sich das Tor bewegt. So bleiben beide Hände außerhalb des Bewegungsbereichs.
- **NOT-AUS:** Ein roter Taster stoppt den Vorgang jederzeit sofort.
- **Audio-Anweisungen (Deutsch, TTS):** Begrüßung, „Druckausgleich läuft", „abgeschlossen", „beide Taster drücken", „Tor offen", „NOT-AUS". Während eine Anweisung läuft, sind die Eingaben gesperrt (kein versehentliches Auslösen).
- **Textuelle Sicherheitshinweise:** Alle Warnungen aus der 360°-Tour werden situationsgerecht als Text auf einem Warn-Panel angezeigt (Inspektion, Vakuum-Warnung, freier Bewegungsbereich, Sichern mit Bolzen usw.).
- **Automatische Kameradrehung:** Nach abgeschlossenem Druckausgleich dreht sich die Blickrichtung zum Tor, damit die Totmann-Taster im Blickfeld liegen.
- **Interaktion:** Hand-Tracking (Virtual Hands) und Controller werden über das Meta XR SDK (Poke Interaction) unterstützt.

**Technischer Hinweis:** Da die verwendete Hardware Meta Quest Link nicht unterstützt, erfolgt das Testen über **Build & Run (APK)** direkt auf dem Headset. Die korrekte VR-Darstellung erforderte das Setzen der Meta-XR-Projekteinstellungen (u. a. Android-API-Level ≥ 29).

## 6. Scope-Hinweise (Soloprojekt)

- **Leitstand** und **Gate** räumlich nah halten oder per Teleport verbinden — **keinen Laufweg modellieren**.
- Vorhandene **goTube-Assets** nutzen; nur die **Kern-Interaktionen** selbst umsetzen.
- Fokus auf den **Lernkern (Vakuum/Druckausgleich)**; Nebenschritte als Checkliste abbilden.

---

### Quellen (für die Concept-Referenzen)
- Institute of Hyperloop Technology / Hochschule Emden/Leer — goTube (27 m, längste Hyperloop-Anlage Deutschlands).
- HyperBRIDGE — Interreg Deutschland-Nederland (Testanlagen goTube/Emden & EHC/Veendam; immersive Trainings).
- Hyperloop Alpha (2013) — Röhren-Betriebsdruck ~100 Pa.
