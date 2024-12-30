pipeline {
    agent any

    stages {
        stage('Checkout Code') {
            steps {
                echo 'Cloning repository...'
                git branch: 'master', url: 'https://github.com/Mohamed86122/Project-payment-gateway'
            }
        }
    }

    post {
        success {
            echo 'Stage completed successfully!'
        }
        failure {
            echo 'Stage failed. Please check the logs.'
        }
    }
}
