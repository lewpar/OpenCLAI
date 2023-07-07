# OpenCLAI
Brings the power of OpenAI to your terminal.

## Note
To use this tool anywhere from the command-line you need to add the openclai bin to your system path.

## Usage
To use any of the OpenAI endpoints you must select a OpenAI option.

| Option | Description |
|---|---|
| chatgpt | AI language model that employs deep learning for human-like conversations, offering information, assistance, and creative solutions. |
| dalle | DALL·E is an AI model that uses deep learning to generate high-quality and diverse images from textual descriptions. |
### ChatGPT
To send a prompt to ChatGPT you need to supply a prompt.
```
openclai -chatgpt -prompt "How many planets are in our solar system?"
```
### DALL-E
To send a prompt to DALL-E you need to supply a prompt/description.
```
openclai -dalle -prompt "A white cat with blue eyes."
```

## Licence
This project uses the [GPL-3.0 Licence](https://choosealicense.com/licenses/gpl-3.0/).
