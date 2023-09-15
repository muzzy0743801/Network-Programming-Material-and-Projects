var express = require('express');
var router = express.Router();
var nodemailer = require('nodemailer');
var config =require('../config');
var transporter = nodemailer.createTransport(config.mailer);


/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('index', { title: 'Express' });
});

router.get('/about', function(req, res, next) {
  res.render('about', { title: 'OnlineEditor' });
});

router.route('/contact')
	.get(function (req, res, next) {
	res.render('contact', { title: 'OnlineEditor' });
	})
	.post(function (req, res, next){
	req.checkBody('name', 'Empty Name'). notEmpty();
	req.checkBody('email', 'Invalid email'). isEmail();
	req.checkBody('message', 'Empty Message'). notEmpty();
	var errors= req.validationErrors();

	if (errors) {

		res.render('contact',{
			title:'OnlineEditor-a platform for sharing code',
			name: req.body.name,
			email: req.body.email,
			message: req.body.message,
			errorMessages: errors
		});
	} else{

		var mailOptions= {
			from: 'OnlineEditor <no-reply@enlineeditor.com>',
			to: 'nppafkiet@gmail.com',
			subject: 'You got a new message from visitor',
			text: req.body.message
		};
		transporter.sendMail(mailOptions,function(error,info){
		if (error) {
			return console.log(error);
		}

		res.render('thank', { title: 'OnlineEditor' });


		})
		


	}
	});

router.get('/login', function(req, res, next) {
  res.render('login', { title: 'login your account' });
});

router.get('/register', function(req, res, next) {
  res.render('register', { title: 'register a new account' });
});

module.exports = router;
