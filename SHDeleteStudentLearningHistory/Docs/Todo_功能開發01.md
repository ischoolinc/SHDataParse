
## Goal

建立「刪除學習歷程成績相關資料」主畫面的預設資料與初始化流程。

完成後，將本次修改內容記錄到：

`刪除學習歷程資料調整0918.md`

---

## Files to Modify

主要修改：

* `Program.cs`
* `frmDelStudentLearningHistory.cs`

如有必要，可調整對應 Designer 屬性，但不要變動其他既有功能與 UI 邏輯。

---

# 1. 傳入目前選取的學生

## Requirement

目前 `Program.cs` 已經會判斷：

```csharp
K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0
```

建立 `frmDelStudentLearningHistory` 時，要將目前選取的學生：

```csharp
K12.Presentation.NLDPanels.Student.SelectedSource
```

傳入表單。

目前：

```csharp
frmDelStudentLearningHistory fDLH =
    new frmDelStudentLearningHistory();
```

調整為將 `SelectedSource` 傳入。

例如：

```csharp
frmDelStudentLearningHistory fDLH =
    new frmDelStudentLearningHistory(
        K12.Presentation.NLDPanels.Student.SelectedSource
    );
```

---

## frmDelStudentLearningHistory

修改 constructor，接收所選學生 ID。

例如：

```csharp
private List<string> _StudentIDs;

public frmDelStudentLearningHistory(List<string> studentIDs)
{
    InitializeComponent();

    _StudentIDs = studentIDs;
}
```

注意：

* 使用與 `K12.Presentation.NLDPanels.Student.SelectedSource` 相容的型別。
* 建議複製一份資料保存，避免外部 SelectedSource 後續改變影響表單。

例如：

```csharp
_StudentIDs = studentIDs == null
    ? new List<string>()
    : new List<string>(studentIDs);
```

後續刪除資料功能統一使用 `_StudentIDs`。

---

# 2. 初始化 cbxSchoolYear

## Requirement

`cbxSchoolYear` 預設值使用：

```csharp
K12.Data.School.DefaultSchoolYear
```

下拉選項提供：

```text
預設學年度 - 3
預設學年度 - 2
預設學年度 - 1
預設學年度
預設學年度 + 1
預設學年度 + 2
預設學年度 + 3
```

例如：

```text
DefaultSchoolYear = 115

選項：
112
113
114
115
116
117
118
```

初始化時：

1. 讀取 `K12.Data.School.DefaultSchoolYear`
2. 轉換成 `int`
3. 清除 `cbxSchoolYear.Items`
4. 加入前後各 3 年，共 7 個選項
5. 最後選取 `DefaultSchoolYear`

參考：

```csharp
int defaultSchoolYear;

if (int.TryParse(K12.Data.School.DefaultSchoolYear, out defaultSchoolYear))
{
    cbxSchoolYear.Items.Clear();

    for (int i = -3; i <= 3; i++)
    {
        cbxSchoolYear.Items.Add((defaultSchoolYear + i).ToString());
    }

    cbxSchoolYear.Text = defaultSchoolYear.ToString();
}
```

不要寫死學年度。

---

# 3. 初始化 cbxSemester

## Requirement

`cbxSemester` 預設值使用：

```csharp
K12.Data.School.DefaultSemester
```

選項只能有：

```text
1
2
```

初始化：

```csharp
cbxSemester.Items.Clear();
cbxSemester.Items.Add("1");
cbxSemester.Items.Add("2");

cbxSemester.Text = K12.Data.School.DefaultSemester;
```

並限制使用者只能從：

```text
1
2
```

中選擇。

建議設定：

```csharp
cbxSemester.DropDownStyle = ComboBoxStyle.DropDownList;
```

不要讓使用者自行輸入其他值。

---

# 4. 初始化 cbxScoreType

## Requirement

`cbxScoreType` 選項固定為以下 10 種：

```text
學期成績(4.2)
補修成績(4.3)
轉學轉科成績(4.4)
重修成績(5.2)
重讀成績(5.3)
學期成績(6.2)
補考成績(6.3)
轉學轉科成績(6.4)
學期成績(預檢)(41.2)
學期成績(預檢)(42.2)
```

初始化時清除原有資料後加入：

```csharp
cbxScoreType.Items.Clear();

cbxScoreType.Items.Add("學期成績(4.2)");
cbxScoreType.Items.Add("補修成績(4.3)");
cbxScoreType.Items.Add("轉學轉科成績(4.4)");
cbxScoreType.Items.Add("重修成績(5.2)");
cbxScoreType.Items.Add("重讀成績(5.3)");
cbxScoreType.Items.Add("學期成績(6.2)");
cbxScoreType.Items.Add("補考成績(6.3)");
cbxScoreType.Items.Add("轉學轉科成績(6.4)");
cbxScoreType.Items.Add("學期成績(預檢)(41.2)");
cbxScoreType.Items.Add("學期成績(預檢)(42.2)");
```

限制使用者只能選擇上述內容：

```csharp
cbxScoreType.DropDownStyle = ComboBoxStyle.DropDownList;
```

不要允許手動輸入其他成績類型。

若目前沒有指定預設成績類型，可以：

```csharp
if (cbxScoreType.Items.Count > 0)
    cbxScoreType.SelectedIndex = 0;
```

也就是預設：

```text
學期成績(4.2)
```

---

# 5. 建議集中初始化

不要把初始化程式散落在多個 Event。

建議建立：

```csharp
private void InitializeFormData()
{
    InitSchoolYear();
    InitSemester();
    InitScoreType();
}
```

並拆成：

```csharp
private void InitSchoolYear()
{
}

private void InitSemester()
{
}

private void InitScoreType()
{
}
```

在表單 Load：

```csharp
private void frmDelStudentLearningHistory_Load(object sender, EventArgs e)
{
    this.MinimumSize = this.MaximumSize = this.Size;

    InitializeFormData();
}
```

保留原本：

```csharp
this.MinimumSize = this.MaximumSize = this.Size;
```

不要移除。

---

# Expected Data Flow

```text
學生清單
    ↓
選取一位或多位學生
    ↓
右鍵
「刪除學習歷程成績相關資料」
    ↓
Program.cs
    ↓
SelectedSource.Count > 0
    ↓
將 SelectedSource 傳入
frmDelStudentLearningHistory
    ↓
保存 Student IDs
    ↓
Form Load
    ↓
InitializeFormData()
    ├─ InitSchoolYear()
    │    └─ DefaultSchoolYear ± 3
    │
    ├─ InitSemester()
    │    └─ 1 / 2
    │
    └─ InitScoreType()
         └─ 固定 10 種成績類型
```

---

# Important Constraints

此次只處理主畫面預設資料與學生傳入。

不要：

* 修改目前權限 GUID。
* 修改右鍵選單名稱。
* 修改 `ShowDialog()` 行為。
* 修改其他學生功能。
* 實作真正刪除資料 SQL。
* 改變既有刪除邏輯。
* 加入其他未要求的成績類型。
* 寫死目前學年度。
* 讓 Semester 可以輸入 1、2 以外的資料。
* 讓 ScoreType 可以輸入固定清單以外的資料。

---

# Acceptance Criteria

完成後確認：

* [x] Program.cs 將目前 `SelectedSource` 傳給 `frmDelStudentLearningHistory`
* [x] 表單可以保存所選學生 IDs
* [x] 選取多位學生時也能正常開啟表單
* [x] `cbxSchoolYear` 顯示 DefaultSchoolYear 前後各 3 年
* [x] `cbxSchoolYear` 預設選取 DefaultSchoolYear
* [x] `cbxSemester` 只有 1、2
* [x] `cbxSemester` 預設選取 DefaultSemester
* [x] `cbxSemester` 無法輸入其他資料
* [x] `cbxScoreType` 完整顯示指定的 10 個選項
* [x] `cbxScoreType` 無法輸入其他內容
* [x] 原本 Exit 按鈕仍可正常關閉
* [x] 原本固定視窗大小功能仍正常
* [x] 專案可以正常編譯
* [x] 不影響其他既有功能

---

# Modification Record

修改完成後新增或更新：

`刪除學習歷程資料調整0918.md`

至少記錄：

1. 修改日期
2. 修改檔案
3. `Program.cs` 傳入 SelectedSource 的調整
4. 表單保存 Student IDs 的方式
5. SchoolYear 初始化方式
6. Semester 初始化方式
7. ScoreType 固定選項
8. ComboBox 限制輸入方式
9. 測試結果
10. 是否有影響其他既有功能

請不要只記錄「完成」，要記錄實際修改內容與測試結果。
