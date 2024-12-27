pipeline {
    agent any

    environment {
        BUILD_CONFIGURATION = 'Release'          // Configuration du build (Release/Debug)
        DOTNET_PROJECT = 'AppTest/AppTest.csproj' // Chemin du fichier .csproj
        DOCKER_IMAGE = 'aspnet-core-webapi'      // Nom de l'image Docker
        DOCKER_TAG = 'latest'                    // Tag de l'image Docker
    }

    stages {
        stage('Checkout Code') {
            steps {
                echo 'Cloning repository...'
                git branch: 'master', url: 'https://github.com/Mohamed86122/Project-payment-gateway'
            }
        }

        stage('Restore Dependencies') {
            steps {
                echo 'Restoring dependencies...'
                sh 'dotnet restore ${DOTNET_PROJECT}'
            }
        }

        stage('Build Project') {
            steps {
                echo 'Building the project...'
                sh 'dotnet build ${DOTNET_PROJECT} -c ${BUILD_CONFIGURATION}'
            }
        }

        stage('Run Tests') {
            steps {
                echo 'Running tests...'
                sh 'dotnet test ${DOTNET_PROJECT} -c ${BUILD_CONFIGURATION}'
            }
        }

        stage('Build Docker Image') {
            steps {
                echo 'Building Docker image...'
                sh 'docker build -t ${DOCKER_IMAGE}:${DOCKER_TAG} .'
            }
        }

        stage('Push Docker Image to DockerHub') {
            steps {
                echo 'Pushing Docker image to DockerHub...'
                withCredentials([usernamePassword(credentialsId: 'dockerhub_credentials', passwordVariable: 'DOCKER_PASSWORD', usernameVariable: 'DOCKER_USERNAME')]) {
                    sh '''
                    echo $DOCKER_PASSWORD | docker login -u $DOCKER_USERNAME --password-stdin
                    docker tag ${DOCKER_IMAGE}:${DOCKER_TAG} $DOCKER_USERNAME/${DOCKER_IMAGE}:${DOCKER_TAG}
                    docker push $DOCKER_USERNAME/${DOCKER_IMAGE}:${DOCKER_TAG}
                    '''
                }
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed. Please check the logs.'
        }
    }
}
