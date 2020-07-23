
exports.up = function(knex) {
    return knex.schema.alterTable('imtb_account', function(t) {
        t.integer('AdminLevel').notNull().defaultTo(0);
        t.integer('DonateRank').notNull().defaultTo(0);;
        t.integer('Level').notNull().defaultTo(1);;
        t.integer('Respect').notNull().defaultTo(0);;
        t.integer('ConnectedTime').notNull().defaultTo(0);;
        t.integer('Money').notNull().defaultTo(0);
        t.integer('Bank').notNull().defaultTo(0);
    });
    
};

exports.down = function(knex) {
  return knex.schema.alterTable('imtb_account', function(t) {
    t.dropColumn('AdminLevel');
    t.dropColumn('DonateRank');
    t.dropColumn('Level');
    t.dropColumn('Respect');
    t.dropColumn('ConnectedTime');
    t.dropColumn('Money');
    t.dropColumn('Bank');
  });
};
