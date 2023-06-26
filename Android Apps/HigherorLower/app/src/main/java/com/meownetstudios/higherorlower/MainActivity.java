package com.meownetstudios.higherorlower;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import java.util.Random;

public class MainActivity extends AppCompatActivity {

    int n;

    public void makeToast(String string){

        Toast.makeText(this, string, Toast.LENGTH_SHORT).show();
    }

    public void guessFunction(View view) {




        EditText editTextUserGuess = (EditText) findViewById(R.id.editTextUserGuess);

        int userGuess = Integer.parseInt(editTextUserGuess.getText().toString());

        if (userGuess > n)
        {
            makeToast("Lower!");
        }
        else if (userGuess < n) {
            makeToast("Higher!");
        }
        else {
            makeToast("That's Right!, Try Again!");

            Random rand = new Random();

            n = rand.nextInt(20) + 1;
        }

    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Random rand = new Random();

        n = rand.nextInt(20) + 1;

    }
}
