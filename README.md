# GptWrapper .Net7
Chat GPT wrapper that makes it easy to use shortcuts for improved workflow.
I've been using this for a while now, and it's been a great help to my workflow. 
If you use Chat GPT in your work, this should save you a lot of time.
Using this you can easily provide context, code or error messages to GPT, without the need of using API and wasting tokens.

## How to use
To use it, login to your Chat-GPT account.
This project uses WebView2 for displaying the site itself, so you always get the newest version.

Settings can be changed/added by changing the `settings.json` located in `%AppData%\Roaming\GptWrapper\`.
You can also change settings at runtime, by using the little settings button in the top right side. The changes are saved on close of the settings panel.

There are two special strings that can be used in the settings:
- `@returnPoint` - specifies where the cursor should be placed after inserting the text in the form.
- `@focusedText` - this text will be replaced with the currently selected text.

### Default hotkeys:
- CTRL+SHIFT+C - provides selected text as context to GPT. Doesn't automatically submit form, you can write your question.
- CTRL+SHIFT+R - provides selected text as code to be refactored by GPT. Automatically submit form.
- CTRL+SHIFT+E - provides selected text as error message to be reviewed by GPT. Automatically submit form
- CTRL+SHIFT+Enter - submits form
- CTRL+SHIFT+W - shuffle window focus.

### Possible modifier keys:
- Alt
- Control
- Shift
- Win

If you need more, you can add them in `ModifierKeys` Enum.

### Installation
If you want to create your own version, you can clone the repo and publish it yourself. 
Then, rebuild the `GptWrapper.Instalator` project, which will produce a `setup.exe` file in the `GptWrapper.Instalator\Release\` folder. 
You can then run the installer and it will install the app for you.

### Example
![Gpt Wrapper](https://github.com/Vegolas/GptWrapper/assets/5692846/e56667eb-906a-491a-85a5-6024f42fb4e5)


