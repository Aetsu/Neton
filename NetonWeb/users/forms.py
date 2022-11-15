from django import forms

class AuthenticationForm(forms.Form):
    username = forms.CharField(
        max_length=254,
        widget=forms.TextInput(attrs={'class': "input"}),
    )
    password = forms.CharField(
        widget=forms.PasswordInput(attrs={'class': "input", 'type': 'password'})
        )
