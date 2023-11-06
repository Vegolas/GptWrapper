# GptWrapper
Chat GPT wrapper that makes it easy to use shortcuts for improved workflow.

## How to use
To use it, login to your Chat-GPT account.
This project uses WebView2 for displaying the site itself, so you always get the newest version.


### Default hotkeys:
- CTRL+SHIFT+C - provides selected text as context to GPT. Doesn't automatically submit form, you can write your question.
- CTRL+SHIFT+R - provides selected text as code to be refactored by GPT. Automatically submit form.
- CTRL+SHIFT+E - provides selected text as error message to be reviewed by GPT. Automatically submit form
- CTRL+SHIFT+Enter - submits form
- CTRL+SHIFT+W - shuffle window focus.

All settings can be changed by changing the settings.json located in `%AppData%\Roaming\GptWrapper\`.
You can also change settings at runtime, by using the little settings button in the top right side. The changes are saved on close of the settings panel.

### Example
![Gpt Wrapper](https://github.com/Vegolas/GptWrapper/assets/5692846/e56667eb-906a-491a-85a5-6024f42fb4e5)
